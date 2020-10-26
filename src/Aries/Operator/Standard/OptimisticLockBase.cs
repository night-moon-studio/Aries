using System;

namespace Aries
{
    /// <summary>
    /// 乐观锁模型
    /// </summary>
    public abstract class OptimisticLockBase
    {

        protected string Name;
        protected long Uid;
        private int _timeOut;
        public int SucceedCount;

        /// <summary>
        /// 指定锁
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="name"></param>
        /// <param name="timeOut"></param>
        public void SpecifyLock(long uid, string name, int timeOut = 15)
        {
            Name = name;
            Uid = uid;
            _timeOut = timeOut;
        }


        /// <summary>
        /// 获取锁数据
        /// </summary>
        /// <returns></returns>
        public virtual AriesOptimisticLockModel GetLock()
        {
            return null;
        }


        /// <summary>
        /// 更新锁
        /// </summary>
        /// <returns></returns>
        public virtual bool UpdateLock()
        {
            return true;
        }


        /// <summary>
        /// 释放锁
        /// </summary>
        /// <returns></returns>
        public virtual bool ReleaseLock()
        {
            return true;
        }


        /// <summary>
        /// 创建锁数据
        /// </summary>
        /// <returns></returns>
        public virtual AriesOptimisticLockModel CreateLock()
        {
            return null;
        }

        public virtual OptimisticLockResult Execute(Action action)
        {

            var temp = GetLock();
            if (temp == null)
            {
                //创建锁
                temp = CreateLock();
                if (temp == null)
                {
                    return OptimisticLockResult.DBError;
                }

            }



            //如果锁超时
            if (temp.IsLocked && NowTimeStampSecond() - temp.LockedTime > _timeOut)
            {
                //强行释放锁
                ReleaseLock();
                //重新获取锁
                temp = GetLock();
                
            }

            if (temp.IsLocked)
            {

                //如果被锁则返回
                return OptimisticLockResult.HasLocked;
                
            }
            else
            {

                //锁空闲则上锁
                if (!UpdateLock())
                {
                    return OptimisticLockResult.HasLocked;
                }
                else
                {

                    action();
                    SucceedCount += 1;
                    ReleaseLock();
                    return OptimisticLockResult.Succeed;

                }

            }

        }


        public static long NowTimeStampSecond()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

    }

    public enum OptimisticLockResult
    {

        Succeed,
        DBError,
        HasLocked,

    }

}
