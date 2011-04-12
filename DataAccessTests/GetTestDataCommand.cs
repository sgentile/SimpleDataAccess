using System;
using System.Collections.ObjectModel;
using System.Data;
using DataAccess;

namespace DataAccessTests
{
    public class GetTestDataCommand : DataCommand<Entities>
    {
        public override MapperBase<Entities> Mapper
        {
            get
            {
                return new TestDataMapper();
            }
        }

        public override string CommandText
        {
            get { return "SELECT * FROM Entities WHERE EntityName = @EntityName"; }
        }

        public override CommandType CommandType
        {
            get { return CommandType.Text; }
        }
    }
}