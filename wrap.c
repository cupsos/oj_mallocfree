#include <stdio.h>
#include <unistd.h>
#include <string.h>

#define STR_BUF_SIZE 1024
static char LOG_PATH[STR_BUF_SIZE];

__attribute__((constructor))
static void initialize(void)
{
	readlink("/proc/self/exe", LOG_PATH, STR_BUF_SIZE);
	char *ptr = LOG_PATH;
	while(*ptr) ptr++;
	while(*ptr != '/') ptr--;
	strcpy(ptr, "/log.txt");
}

void* __real_malloc(size_t size);
void __real_free(void *ptr);
int __MALLOC_COUNT = 0;
int __FREE_COUNT = 0;

void WRITE_LOG(char *str)
{
    FILE *file = fopen(LOG_PATH, "a");
	fputs(str, file);
	fclose(file);
}

void* __wrap_malloc(size_t size)
{
	char str[STR_BUF_SIZE];
	void* ret = __real_malloc(size);
	sprintf(str, "malloc : %p\n", ret);
	WRITE_LOG(str);
	return ret;
}

void __wrap_free(void *ptr)
{
	char str[STR_BUF_SIZE];
	sprintf(str, "free : %p\n", ptr);
	WRITE_LOG(str);
	__real_free(ptr);
}