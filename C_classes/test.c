#include <stdio.h>
#include <stdlib.h>

int fucn1(int p)
{
    return p+1;
}


float average(float age[])
{
	int i;
	float avg, sum = 0.0;
	for (i = 0; i < 6; ++i) {
		sum += age[i];
	}
	avg = (sum / 6);
	return avg;
}

int main()
{

    void* f1 = &fucn1;
    int b = -10;
    int a = ((int(*)(int))f1)(b);
    printf("%d",a);
    return 0;
}

