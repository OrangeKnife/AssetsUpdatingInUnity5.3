namespace LOM
{
    public class EventObj
    {
        public string eventName;
        public int paramInt;
        public float paramFloat;
        public string paramString;

        public EventObj(string name)
        {
            eventName = name;
            paramInt = -1;
            paramFloat = -1f;
            //paramString = "";
        }
        public EventObj(string name, int pInt)
        {
            eventName = name;
            paramInt = pInt;
            paramFloat = -1f;
            //paramString = "";
        }
        public EventObj(string name, string pString)
        {
            eventName = name;
            paramInt = -1;
            paramFloat = -1f;
            paramString = pString;
        }
        public EventObj(string name, string pString, int pInt)
        {
            eventName = name;
            paramInt = pInt;
            paramFloat = -1f;
            paramString = pString;
        }
        public EventObj(string name, string pString, int pInt, float pFloat)
        {
            eventName = name;
            paramInt = pInt;
            paramFloat = pFloat;
            paramString = pString;
        }
    }

    public class EventObj_2 : EventObj
    {
        public int paramInt2;
        public float paramFloat2;
        public string paramString2;

        public EventObj_2(string name, string pString, string pString2, int pInt, int pInt2, float pFloat, float pFloat2)
            :base(name, pString, pInt, pFloat)
        {
            paramString2 = pString2;
            paramInt2 = pInt2;
            paramFloat2 = pFloat2;
        }
    }
}
