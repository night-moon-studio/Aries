namespace Aries
{
    public class AriesOptimisticLock : OptimisticLockBase
    {


        private readonly IFreeSql _freeSql;

        public AriesOptimisticLock()
        {

        }
        public AriesOptimisticLock(IFreeSql freeSql)
        {
            _freeSql = freeSql;
        }


        public override AriesOptimisticLockModel CreateLock()
        {

            try
            {
                var model = new AriesOptimisticLockModel() { Uid = this.Uid, Name = this.Name, LockedTime = NowTimeStampSecond() };
                return _freeSql.AriesInsert(model) ? model : null;
            }
            catch (System.Exception)
            {
                return null;
            }
            
        }




        public override AriesOptimisticLockModel GetLock()
        {
            return _freeSql.Select<AriesOptimisticLockModel>().Where(item => item.Uid == Uid && item.Name == Name).First();
        }


        public override bool UpdateLock()
        {
            
            return _freeSql
                .Update<AriesOptimisticLockModel>()
                .Set(item=>item.IsLocked == true)
                .Set(item=>item.LockedTime == NowTimeStampSecond())
                .Where(item => item.Uid == Uid && item.Name == Name && item.IsLocked == false).ExecuteAffrows()==1;

        }

        public override bool ReleaseLock()
        {
            return _freeSql
                .Update<AriesOptimisticLockModel>()
                .Set(item => item.IsLocked == false)
                .Set(item => item.LockedTime == NowTimeStampSecond())
                .Where(item => item.Uid == Uid && item.Name == Name).ExecuteAffrows() == 1;
        }
    }
}
