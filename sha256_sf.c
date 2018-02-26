
#include <stddef.h>
#include <stdint.h>
#include <string.h>
#include <stdio.h>
#include <stdlib.h>
typedef struct
{
    uint32_t total[2];          /*!< number of bytes processed  */
    uint32_t state[8];          /*!< intermediate digest state  */
    unsigned char buffer[64];   /*!< data block being processed */
    int is224;                  /*!< 0 => SHA-256, else SHA-224 */
}
mbedtls_sha256_context;
#define mbedtls_printf printf
#define mbedtls_calloc    calloc
#define mbedtls_free       free



/* Implementation that should never be optimized out by the compiler */
static void mbedtls_zeroize( void *v, size_t n ) {
    volatile unsigned char *p = v; while( n-- ) *p++ = 0;
}

/*
 * 32-bit integer manipulation macros (big endian)
 */
#ifndef GET_UINT32_BE
#define GET_UINT32_BE(n,b,i)                            \
do {                                                    \
    (n) = ( (uint32_t) (b)[(i)    ] << 24 )             \
        | ( (uint32_t) (b)[(i) + 1] << 16 )             \
        | ( (uint32_t) (b)[(i) + 2] <<  8 )             \
        | ( (uint32_t) (b)[(i) + 3]       );            \
} while( 0 )
#endif

#ifndef PUT_UINT32_BE
#define PUT_UINT32_BE(n,b,i)                            \
do {                                                    \
    (b)[(i)    ] = (unsigned char) ( (n) >> 24 );       \
    (b)[(i) + 1] = (unsigned char) ( (n) >> 16 );       \
    (b)[(i) + 2] = (unsigned char) ( (n) >>  8 );       \
    (b)[(i) + 3] = (unsigned char) ( (n)       );       \
} while( 0 )
#endif

void mbedtls_sha256_init( mbedtls_sha256_context *ctx )
{
    memset( ctx, 0, sizeof( mbedtls_sha256_context ) );
}

void mbedtls_sha256_free( mbedtls_sha256_context *ctx )
{
    if( ctx == NULL )
        return;

    mbedtls_zeroize( ctx, sizeof( mbedtls_sha256_context ) );
}

void mbedtls_sha256_clone( mbedtls_sha256_context *dst,
                           const mbedtls_sha256_context *src )
{
    *dst = *src;
}

/*
 * SHA-256 context setup
 */
void mbedtls_sha256_starts( mbedtls_sha256_context *ctx)
{
    ctx->total[0] = 0;
    ctx->total[1] = 0;
        /* SHA-256 */
        ctx->state[0] = 1779033703;
        ctx->state[1] = -1150833019;
        ctx->state[2] = 1013904242;
        ctx->state[3] = -1521486534;
        ctx->state[4] = 1359893119;
        ctx->state[5] = -1694144372;
        ctx->state[6] = 528734635;
        ctx->state[7] = 1541459225;

    ctx->is224 = 0;
}

static const uint32_t K[] =
{
    0x428A2F98, 0x71374491, 0xB5C0FBCF, 0xE9B5DBA5,
    0x3956C25B, 0x59F111F1, 0x923F82A4, 0xAB1C5ED5,
    0xD807AA98, 0x12835B01, 0x243185BE, 0x550C7DC3,
    0x72BE5D74, 0x80DEB1FE, 0x9BDC06A7, 0xC19BF174,
    0xE49B69C1, 0xEFBE4786, 0x0FC19DC6, 0x240CA1CC,
    0x2DE92C6F, 0x4A7484AA, 0x5CB0A9DC, 0x76F988DA,
    0x983E5152, 0xA831C66D, 0xB00327C8, 0xBF597FC7,
    0xC6E00BF3, 0xD5A79147, 0x06CA6351, 0x14292967,
    0x27B70A85, 0x2E1B2138, 0x4D2C6DFC, 0x53380D13,
    0x650A7354, 0x766A0ABB, 0x81C2C92E, 0x92722C85,
    0xA2BFE8A1, 0xA81A664B, 0xC24B8B70, 0xC76C51A3,
    0xD192E819, 0xD6990624, 0xF40E3585, 0x106AA070,
    0x19A4C116, 0x1E376C08, 0x2748774C, 0x34B0BCB5,
    0x391C0CB3, 0x4ED8AA4A, 0x5B9CCA4F, 0x682E6FF3,
    0x748F82EE, 0x78A5636F, 0x84C87814, 0x8CC70208,
    0x90BEFFFA, 0xA4506CEB, 0xBEF9A3F7, 0xC67178F2,
};

#define  SHR(x,n) ((x & 0xFFFFFFFF) >> n)
#define ROTR(x,n) (SHR(x,n) | (x << (32 - n)))

#define S0(x) (ROTR(x, 7) ^ ROTR(x,18) ^  SHR(x, 3))
#define S1(x) (ROTR(x,17) ^ ROTR(x,19) ^  SHR(x,10))

#define S2(x) (ROTR(x, 2) ^ ROTR(x,13) ^ ROTR(x,22))
#define S3(x) (ROTR(x, 6) ^ ROTR(x,11) ^ ROTR(x,25))

#define F0(x,y,z) ((x & y) | (z & (x | y)))
#define F1(x,y,z) (z ^ (x & (y ^ z)))

#define R(t)                                    \
(                                               \
    W[t] = S1(W[t -  2]) + W[t -  7] +          \
           S0(W[t - 15]) + W[t - 16]            \
)

#define P(a,b,c,d,e,f,g,h,x,K)                  \
{                                               \
    temp1 = h + S3(e) + F1(e,f,g) + K + x;      \
    temp2 = S2(a) + F0(a,b,c);                  \
    d += temp1; h = temp1 + temp2;              \
}

void mbedtls_sha256_process( mbedtls_sha256_context *ctx, const unsigned char data[64] )
{
    uint32_t temp1, temp2, W[64];
    uint32_t A[8];
    unsigned int i;

    for( i = 0; i < 8; i++ )
        A[i] = ctx->state[i];

#if defined(MBEDTLS_SHA256_SMALLER)
    for( i = 0; i < 64; i++ )
    {
        if( i < 16 )
            GET_UINT32_BE( W[i], data, 4 * i );
        else
            R( i );

        P( A[0], A[1], A[2], A[3], A[4], A[5], A[6], A[7], W[i], K[i] );

        temp1 = A[7]; A[7] = A[6]; A[6] = A[5]; A[5] = A[4]; A[4] = A[3];
        A[3] = A[2]; A[2] = A[1]; A[1] = A[0]; A[0] = temp1;
    }
#else /* MBEDTLS_SHA256_SMALLER */
    for( i = 0; i < 16; i++ )
        GET_UINT32_BE( W[i], data, 4 * i );

    for( i = 0; i < 16; i += 8 )
    {
        P( A[0], A[1], A[2], A[3], A[4], A[5], A[6], A[7], W[i+0], K[i+0] );
        P( A[7], A[0], A[1], A[2], A[3], A[4], A[5], A[6], W[i+1], K[i+1] );
        P( A[6], A[7], A[0], A[1], A[2], A[3], A[4], A[5], W[i+2], K[i+2] );
        P( A[5], A[6], A[7], A[0], A[1], A[2], A[3], A[4], W[i+3], K[i+3] );
        P( A[4], A[5], A[6], A[7], A[0], A[1], A[2], A[3], W[i+4], K[i+4] );
        P( A[3], A[4], A[5], A[6], A[7], A[0], A[1], A[2], W[i+5], K[i+5] );
        P( A[2], A[3], A[4], A[5], A[6], A[7], A[0], A[1], W[i+6], K[i+6] );
        P( A[1], A[2], A[3], A[4], A[5], A[6], A[7], A[0], W[i+7], K[i+7] );
    }

    for( i = 16; i < 64; i += 8 )
    {
        P( A[0], A[1], A[2], A[3], A[4], A[5], A[6], A[7], R(i+0), K[i+0] );
        P( A[7], A[0], A[1], A[2], A[3], A[4], A[5], A[6], R(i+1), K[i+1] );
        P( A[6], A[7], A[0], A[1], A[2], A[3], A[4], A[5], R(i+2), K[i+2] );
        P( A[5], A[6], A[7], A[0], A[1], A[2], A[3], A[4], R(i+3), K[i+3] );
        P( A[4], A[5], A[6], A[7], A[0], A[1], A[2], A[3], R(i+4), K[i+4] );
        P( A[3], A[4], A[5], A[6], A[7], A[0], A[1], A[2], R(i+5), K[i+5] );
        P( A[2], A[3], A[4], A[5], A[6], A[7], A[0], A[1], R(i+6), K[i+6] );
        P( A[1], A[2], A[3], A[4], A[5], A[6], A[7], A[0], R(i+7), K[i+7] );
    }
#endif /* MBEDTLS_SHA256_SMALLER */

    for( i = 0; i < 8; i++ )
        ctx->state[i] += A[i];
}

/*
 * SHA-256 process buffer
 */
void mbedtls_sha256_update( mbedtls_sha256_context *ctx, const unsigned char *input,
                    size_t ilen )
{
    size_t fill;
    uint32_t left;

    if( ilen == 0 )
        return;

    left = ctx->total[0] & 0x3F;
    fill = 64 - left;

    ctx->total[0] += (uint32_t) ilen;
    ctx->total[0] &= 0xFFFFFFFF;

    if( ctx->total[0] < (uint32_t) ilen )
        ctx->total[1]++;

    if( left && ilen >= fill )
    {
        memcpy( (void *) (ctx->buffer + left), input, fill );
        mbedtls_sha256_process( ctx, ctx->buffer );
        input += fill;
        ilen  -= fill;
        left = 0;
    }

    while( ilen >= 64 )
    {
        mbedtls_sha256_process( ctx, input );
        input += 64;
        ilen  -= 64;
    }

    if( ilen > 0 )
        memcpy( (void *) (ctx->buffer + left), input, ilen );
}

static const unsigned char sha256_padding[64] =
{
 0x80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
};

/*
 * SHA-256 final digest
 */
void mbedtls_sha256_finish( mbedtls_sha256_context *ctx, unsigned char output[32] )
{
    uint32_t last, padn;
    uint32_t high, low;
    unsigned char msglen[8];

    high = ( ctx->total[0] >> 29 )
         | ( ctx->total[1] <<  3 );
    low  = ( ctx->total[0] <<  3 );

    PUT_UINT32_BE( high, msglen, 0 );
    PUT_UINT32_BE( low,  msglen, 4 );

    last = ctx->total[0] & 0x3F;
    padn = ( last < 56 ) ? ( 56 - last ) : ( 120 - last );

    mbedtls_sha256_update( ctx, sha256_padding, padn );
    mbedtls_sha256_update( ctx, msglen, 8 );

    PUT_UINT32_BE( ctx->state[0], output,  0 );
    PUT_UINT32_BE( ctx->state[1], output,  4 );
    PUT_UINT32_BE( ctx->state[2], output,  8 );
    PUT_UINT32_BE( ctx->state[3], output, 12 );
    PUT_UINT32_BE( ctx->state[4], output, 16 );
    PUT_UINT32_BE( ctx->state[5], output, 20 );
    PUT_UINT32_BE( ctx->state[6], output, 24 );

    if( ctx->is224 == 0 )
        PUT_UINT32_BE( ctx->state[7], output, 28 );
}

/*
 * output = SHA-256( input buffer )
 */
void mbedtls_sha256( const unsigned char *input, size_t ilen,
             unsigned char output[32] )
{
    mbedtls_sha256_context ctx;

    mbedtls_sha256_init( &ctx );
    mbedtls_sha256_starts( &ctx );
    mbedtls_sha256_update( &ctx, input, ilen );
    mbedtls_sha256_finish( &ctx, output );
    mbedtls_sha256_free( &ctx );
}
int create_license_file(const unsigned char *checksum, FILE *fp)
{
char line_break[] = {0x0A,0};
char* cs_start_label = "<checksum>";
char* cs_end_label = "</checksum>";
int sumlen = 32 * 2;
int len = 0;
int i = 0;
unsigned char *copysum = NULL;

copysum = (unsigned char *)mbedtls_calloc(sumlen, sizeof(unsigned char));
memset(copysum, 0, sumlen);

for(i = 0; i < sumlen; i+=2)
{
	sprintf(copysum + i, "%02x", *(checksum + i/2));
}
len = fwrite(cs_start_label, sizeof(char), strlen(cs_start_label), fp);
len += fwrite(copysum, sizeof(unsigned char), sumlen, fp);
len += fwrite(cs_end_label, sizeof(char), strlen(cs_end_label), fp);
return len;
}
int main(int argc, char * argv[])
{

    int i, j, padding_len=216, padding_index=0, padding_strlen=0,buflen = 0;
    unsigned char *buf;
    unsigned char sha256sum[32];
    unsigned char padding_buf[216];
unsigned char fixdata[] = {
	0x54, 0x00, 0x00, 0x00, 0x69, 0x00, 0x00, 0x00, 0x6B, 0x00, 0x00, 0x00, 0x76, 0x00, 0x00, 0x00, 
	0x24, 0x00, 0x00, 0x00, 0x6E, 0x00, 0x00, 0x00, 0x79, 0x00, 0x00, 0x00, 0x27, 0x00, 0x00, 0x00, 
	0x7C, 0x00, 0x00, 0x00, 0x71, 0x00, 0x00, 0x00, 0x6F, 0x00, 0x00, 0x00, 0x2B, 0x00, 0x00, 0x00, 
	0x7F, 0x00, 0x00, 0x00, 0x82, 0xFF, 0xFF, 0xFF, 0x7E, 0x00, 0x00, 0x00, 0x74, 0x00, 0x00, 0x00, 
	0x82, 0xFF, 0xFF, 0xFF, 0x31, 0x00, 0x00, 0x00, 0x85, 0xFF, 0xFF, 0xFF, 0x78, 0x00, 0x00, 0x00, 
	0x77, 0x00, 0x00, 0x00, 0x87, 0xFF, 0xFF, 0xFF, 0x7B, 0x00, 0x00, 0x00, 0x8B, 0xFF, 0xFF, 0xFF, 
	0x38, 0x00, 0x00, 0x00, 0x82, 0xFF, 0xFF, 0xFF, 0x88, 0xFF, 0xFF, 0xFF, 0x8F, 0xFF, 0xFF, 0xFF, 
	0x85, 0xFF, 0xFF, 0xFF, 0x7E, 0x00, 0x00, 0x00, 0x8A, 0xFF, 0xFF, 0xFF, 0x3F, 0x00, 0x00, 0x00, 
	0x93, 0xFF, 0xFF, 0xFF, 0x95, 0xFF, 0xFF, 0xFF, 0x94, 0xFF, 0xFF, 0xFF, 0x8C, 0xFF, 0xFF, 0xFF, 
	0x92, 0xFF, 0xFF, 0xFF, 0x8C, 0xFF, 0xFF, 0xFF, 0x46, 0x00, 0x00, 0x00, 0x67, 0x00, 0x00, 0x00, 
	0x49, 0x00, 0x00, 0x00, 0x72, 0x00, 0x00, 0x00, 0x4D, 0x00, 0x00, 0x00, 0x80, 0xFF, 0xFF, 0xFF, 
	0x56, 0x00, 0x00, 0x00, 0x55, 0x00, 0x00, 0x00, 0x4F, 0x00, 0x00, 0x00, 0x77, 0x00, 0x00, 0x00, 
	0x74, 0x00, 0x00, 0x00, 0x79, 0x00, 0x00, 0x00, 0x78, 0x00, 0x00, 0x00, 0x54, 0x00, 0x00, 0x00, 
	0x5E, 0x00, 0x00, 0x00, 0x35, 0x00, 0x00, 0x00
};

    int file_size;
    mbedtls_sha256_context ctx;
    FILE *fp;
	if(argc < 1){
	
        mbedtls_printf("%d", argc);
        mbedtls_printf("Arguments error.");
        mbedtls_printf("\n");
        exit(0);
}
    fp = fopen(argv[1], "ab+");
    if(fp == NULL)
     {
        mbedtls_printf("No file.");
        mbedtls_printf("\n");
        exit(0);
     }
    fseek(fp, 0, SEEK_END);
    file_size = ftell(fp);
    fseek(fp, 0, SEEK_SET);
    buf = (unsigned char *)mbedtls_calloc(file_size, sizeof(unsigned char));
    memset(buf, 0, file_size);
    buflen = fread(buf, sizeof(unsigned char), file_size, fp);

  memcpy(padding_buf, fixdata, padding_len);
  do
  {
    padding_buf[padding_index] = *((uint32_t *)padding_buf + padding_index) - padding_index;
    ++padding_index;
  }
  while ( padding_index < 54);
  padding_strlen = strlen(padding_buf);
    mbedtls_sha256_init(&ctx);
    mbedtls_sha256_starts(&ctx);
    mbedtls_sha256_update(&ctx, padding_buf, padding_strlen);
    mbedtls_sha256_update(&ctx, buf, buflen);
    mbedtls_sha256_finish(&ctx, sha256sum);
    
for(i = 0; i < 32; i++)
{
    mbedtls_printf("%02x", sha256sum[i]);
}
    mbedtls_printf("\n");
buflen = create_license_file((unsigned char *)sha256sum, fp);

    mbedtls_printf("total append %d byte.\n", buflen);
    mbedtls_sha256_free(&ctx);
    mbedtls_free(buf);
    fclose(fp);

    return 0;
}

