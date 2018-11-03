namespace Retypeit.Scripts.Bindings.Tests.Mock
{
    public class InvokerFunctionsMock
    {
        public string UCase(string value)
        {
            return value.ToUpperInvariant();
        }

        public int AddInt(int v1, int v2)
        {
            return v1 + v2;
        }

        public decimal AddIntDec(int v1, decimal v2)
        {
            return v1 + v2;
        }

        public int One()
        {
            return 1;
        }

        public void VoidMethod()
        {
        }

        public int AddWithOptionalParams(int p1 = 1, int p2 = 2)
        {
            return p1 + p2;
        }
    }
}