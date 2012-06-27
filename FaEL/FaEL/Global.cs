static class GlobalClass
{
    //users ID
    private static int m_globalID = 0;

    //users Password
    private static string m_globalPASS = " ";

    //Selected Friends ID
    private static string m_friendID = " ";

    private static int m_globalLEN = 0;

    private static string m_sfriendID = " ";

    public static int GLOBALID
    {
        get { return m_globalID; }
        set { m_globalID = value; }
    }

    public static int GLOBALLEN
    {
        get { return m_globalLEN; }
        set { m_globalLEN = value; }
    }

    public static string FRIENDID
    {
        get { return m_friendID; }
        set { m_friendID = value; }
    }

    public static string sFRIENDID
    {
        get { return m_sfriendID; }
        set { m_sfriendID = value; }
    }

     public static string GLOBALPASS
    {
        get { return m_globalPASS; }
        set { m_globalPASS = value; }
    }

     public struct FArray
     {
         public string FID;
         public string FName;
     }

     public static FArray[] friendsArray = new FArray[50];
}