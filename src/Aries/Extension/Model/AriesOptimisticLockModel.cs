public class AriesOptimisticLockModel
{

    public long Uid { get; set; }
    public long LockedTime { get; set; }
    public string Name { get; set; }
    public bool IsLocked { get; set; }

}
