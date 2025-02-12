using InostyProject.Models;

namespace InostyProject.Data
{
    public class SortMembers
    {
        public static List<MemberDataModel> Sort(List<MemberDataModel> members, string sortType)
        {
            if (members.Count < 1)
            {
                return members;
            }

            if (sortType != "Ascending" && sortType != "Descending")
            {
                Console.WriteLine("Ascending or Descending are the sort types.");
                return members;
            }

            QuickSort(members, 0, members.Count - 1, sortType);

            return members;
        }
        
        private static void Swap(List<MemberDataModel> members, int index1, int index2)
        {
            (members[index1], members[index2]) = (members[index2], members[index1]);
        }

        private static int Partition(List<MemberDataModel> members, int lowPoint, int highPoint, string sortType)
        {

            MemberDataModel memberPivot = members[highPoint];
            int lastPoint = lowPoint - 1;

            for (int i = lowPoint; i < highPoint; i++)
            {
                if (sortType == "Ascending")
                {
                    if (members[i].AccountName.CompareTo(memberPivot.AccountName) < 0)
                    {
                        lastPoint++;
                        Swap(members, lastPoint, i);
                    }
                }
                else
                {
                    if (members[i].AccountName.CompareTo(memberPivot.AccountName) > 0)
                    {
                        lastPoint++;
                        Swap(members, lastPoint, i);
                    }
                }
            }

            Swap(members, lastPoint + 1, highPoint);

            return lastPoint + 1;
        }

        private static void QuickSort(List<MemberDataModel> members, int lowPoint, int highPoint, string sortType)
        {
            if (lowPoint > highPoint)
            {
                return;
            }

            int pivot = Partition(members, lowPoint, highPoint, sortType);
            QuickSort(members, lowPoint, pivot - 1, sortType);
            QuickSort(members, pivot + 1, highPoint, sortType);
        }
    }
}
