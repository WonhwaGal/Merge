public struct MergeEvent: IGameEvent
{
    public int MergingRank;

    public MergeEvent(int mergingRank) => MergingRank = mergingRank;
}