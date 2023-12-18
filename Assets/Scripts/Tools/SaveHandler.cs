using Code.DropLogic;
using System.Collections.Generic;
using static Code.SaveLoad.ProgressData;

namespace Code.SaveLoad
{
    public class SaveHandler
    {
        private readonly List<DropSave> _drops = new();

        public List<DropSave> Drops => _drops;

        public void FillData(DropObject drop) 
            => _drops.Add(new DropSave(drop.Rank, drop.transform.position));

        public void Clear() => _drops.Clear();
    }
}