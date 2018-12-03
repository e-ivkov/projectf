#include <stdio.h>
#include <stdlib.h>

/*
    float
    int
    bool
    string
    Later complex
    Later rational
*/

void print_float(float value)
{
    printf("%f", value);
}

void print_int(int value)
{
    printf("%d", value);
}

void print_bool(int value)
{
    if (value != 0)
    {
        printf("true");
    }
    else
    {
        printf("false");
    }
}
void print_string(char* value){
    int i = 0;
    char symbol = value[i];
    while(symbol!= '\0')
    {
        printf("%c",symbol);
        i++;
        symbol = value[i];
    }
}
main(int argc, char const *argv[])
{
    print_float(1.2);
    printf("\n");
    print_int(1);
    printf("\n");
    print_bool(1);
    printf("\n");
    print_bool(0);
    printf("\n");
    print_string("HAHHA");
}
