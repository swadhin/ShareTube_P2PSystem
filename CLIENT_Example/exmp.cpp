#include<iostream>
using namespace std;
inline int factorial(int n)
{
    if(!n) return 1;
    else return n*factorial(n-1);
}
int main()
{
int k= factorial(5);
cout<<k;
return 0;

}

