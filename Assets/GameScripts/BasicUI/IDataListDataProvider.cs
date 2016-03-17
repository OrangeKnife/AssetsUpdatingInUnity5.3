using System.Collections.Generic;

namespace LOM
{
    public interface IDataListDataProvider
    {
        List<MetaDataForWidget> GetData(int dataCategory);
    }
}
