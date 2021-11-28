using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;

namespace DalObject
{
    public partial class DalObject : IDal.DO.IDal
    {
        public Predicate<T> GetObjByIdP<T>(Predicate<T> findBy)
        {
            return findBy;
        }
        public IEnumerable<Parcel> GetAllMatchedEntities(Func<Parcel, Boolean> predicate)
        {
            return displayParcels().Where<Parcel>(predicate);
        }
    }      
}
   