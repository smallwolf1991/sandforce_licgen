#undef   _FILE_OFFSET_BITS   
#define   _FILE_OFFSET_BITS   64   
#include <stdio.h>

#include "mbedtls/aes.h"
const int ARGUMENT_LENGTH = 3;
const int BLOCK_SIZE = 1024 * 1024 * 4;

unsigned char * buff = NULL;
unsigned char * decrypt_buff = NULL;

unsigned char iv[16] = {0x4A, 0x8D, 0x46, 0x52, 0xB3, 0x56, 0xED, 0xD8, 0x17, 0x5A, 0x9D, 0xB1, 0x3E, 0x69, 0x1B, 0x32};
unsigned char key[16] = {0x0B, 0x3F, 0x3E, 0xDD, 0x55, 0xA4, 0xC8, 0x85, 0x34, 0x24, 0x15, 0x3E, 0xD7, 0x87, 0xA9, 0x5A};
int preview(unsigned char * buff)
{
	int i = 0;
	printf("preview top 16 bytes for read file: ");
	for(; i < 16; i++)
	{
		printf("0x%02X ", *(buff + i));
	}
	printf("\n");
}

int decrypt_block(int offset, FILE *fp, FILE *ofp, mbedtls_aes_context *ctx)
{
	int result = 0;
	int read_count = 0;
	
	
	memset(buff, 0, BLOCK_SIZE);
	memset(decrypt_buff, 0, BLOCK_SIZE);
	
	read_count = fread(buff, sizeof(unsigned char), BLOCK_SIZE, fp);
	
	if(offset == 0)
	{
		preview(buff);
	}
	
	result = mbedtls_aes_crypt_cbc( ctx, MBEDTLS_AES_DECRYPT, sizeof(unsigned char) * BLOCK_SIZE, iv, buff, decrypt_buff );
	if(result != 0)
	{
		return result;
	}
	fwrite(decrypt_buff, sizeof(unsigned char), read_count, ofp);
	return 0;
}
int main(int argc,char* argv[])
{
	int file_size = 0;
	int result = 0;
	int offset = 0;
    mbedtls_aes_context ctx;
    FILE *fp;
    FILE *ofp;
	
	
	if(argc < ARGUMENT_LENGTH)
	{
		
		printf("Arguments error! except %d, but is %d\n", ARGUMENT_LENGTH, argc);
		return 0;
	}
    fp = fopen(argv[1], "rb");
    if(fp == NULL)
    {
        printf("No file.\n");
		return 0;
    }
	
    ofp = fopen(argv[2], "wb");
    if(ofp == NULL)
    {
        printf("No file.\n");
		return 0;
    }
	
    fseek(fp, 0, SEEK_END);
    file_size = ftell(fp);
    fseek(fp, 0, SEEK_SET);
	
	
	printf("Read file size: %d\n", file_size);
	
	buff = (unsigned char *) malloc(sizeof(unsigned char) * BLOCK_SIZE);
	decrypt_buff = (unsigned char *) malloc(sizeof(unsigned char) * BLOCK_SIZE);
	
    mbedtls_aes_init( &ctx );
	
	mbedtls_aes_setkey_dec( &ctx, key, 128);
	
	while(offset * BLOCK_SIZE < file_size)
	{
		printf("decrypting %d block...\n", offset + 1);
		result = decrypt_block(offset++, fp, ofp, &ctx);
		if(result != 0)
		{
			printf("Decrypt fail! [code:%d]\n", result);
			goto exit;
		}
		printf("decrypted %d block!\n", offset + 1);
	}
	
	printf("Save file done!\n");
	
	
exit:
    mbedtls_aes_free( &ctx );
	free(buff);
	free(decrypt_buff);
	fclose(fp);
	fclose(ofp);
}