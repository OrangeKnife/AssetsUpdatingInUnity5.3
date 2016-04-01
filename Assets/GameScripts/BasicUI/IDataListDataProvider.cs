namespace LOM
{
    using System.Collections.Generic;

    public interface IDataListDataProvider
    {
        List<MetaDataForWidget> GetData(int dataCategory);
    }
}
