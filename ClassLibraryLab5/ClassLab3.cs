using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryLab5
{
    public static class ClassLab3
    {
        public static long Infinity = 100000000000;
        public static List<List<Tuple<long, long>>> roomConnections = new List<List<Tuple<long, long>>>(2000);
        public static List<Edge> doors = new List<Edge>();
        public static List<long> knowledgeRating = new List<long>(2000);
        public static List<long> previousRoom = new List<long>(2000);
        public static List<long> usedRooms = new List<long>(2000);
        public static List<long> blockedRooms = new List<long>(2000);

        public struct Edge
        {
            public long from, to, knowledgeChange;
        }

        public static void MarkVisitedRooms(long currentRoom, long groupNumber)
        {
            usedRooms[(int)currentRoom] = groupNumber;
            foreach (var connection in roomConnections[(int)currentRoom])
            {
                if (usedRooms[(int)connection.Item1] == 0)
                    MarkVisitedRooms(connection.Item1, groupNumber);
            }
        }

        public static string Execute(string inputString)
        {
            long numberOfRooms, numberOfDoors;
            string[] splittedSrting = inputString.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            numberOfRooms = long.Parse(splittedSrting[0]);
            numberOfDoors = long.Parse(splittedSrting[1]);

            for (int i = 0; i < 2000; i++)
            {
                roomConnections.Add(new List<Tuple<long, long>>());
                knowledgeRating.Add(-Infinity);
                previousRoom.Add(-1);
                usedRooms.Add(0);
                blockedRooms.Add(0);
            }

            string[] extraData = new string[3 * numberOfDoors];
            Array.Copy(splittedSrting, 2, extraData, 0, extraData.Length);

            for (int i = 0; i < numberOfDoors; i += 3)
            {
                string[] doorInfo = [extraData[i], extraData[i + 1], extraData[i + 2]];
                Edge door = new Edge();
                door.from = long.Parse(doorInfo[0]);
                door.to = long.Parse(doorInfo[1]);
                door.knowledgeChange = long.Parse(doorInfo[2]);
                door.from--; door.to--;
                roomConnections[(int)door.to].Add(new Tuple<long, long>(door.from, door.knowledgeChange));
                doors.Add(door);
            }

            knowledgeRating[0] = 0;

            for (int k = 0; k < numberOfRooms; k++)
            {
                foreach (var door in doors)
                {
                    if (knowledgeRating[(int)door.from] > -Infinity && knowledgeRating[(int)door.to] < knowledgeRating[(int)door.from] + door.knowledgeChange)
                    {
                        knowledgeRating[(int)door.to] = knowledgeRating[(int)door.from] + door.knowledgeChange;
                        previousRoom[(int)door.to] = door.from;
                    }
                }
            }

            if (knowledgeRating[(int)numberOfRooms - 1] == -Infinity)
            {
                return ":(";
            }

            MarkVisitedRooms(numberOfRooms - 1, 1);

            long currentRoom = 2000;
            while (currentRoom != -1)
            {
                currentRoom = -1;
                foreach (var door in doors)
                {
                    if (blockedRooms[(int)door.from] == 0 && blockedRooms[(int)door.to] == 0)
                    {
                        if (knowledgeRating[(int)door.from] > -Infinity && knowledgeRating[(int)door.to] < knowledgeRating[(int)door.from] + door.knowledgeChange)
                        {
                            knowledgeRating[(int)door.to] = knowledgeRating[(int)door.from] + door.knowledgeChange;
                            previousRoom[(int)door.to] = door.from;
                            currentRoom = door.from;
                        }
                    }
                }

                if (currentRoom == -1) continue;

                List<long> cycle = new List<long>();
                long y = currentRoom;
                for (int i = 0; i < numberOfRooms; i++)
                {
                    y = previousRoom[(int)y];
                }

                for (long cur = y; ; cur = previousRoom[(int)cur])
                {
                    cycle.Add(cur);
                    if (cur == y && cycle.Count > 1) break;
                }

                foreach (var room in cycle)
                {
                    blockedRooms[(int)room] = 1;
                }
            }

            for (int i = 0; i < numberOfRooms; i++)
            {
                if (usedRooms[(int)i] != 0 && blockedRooms[(int)i] != 0)
                {
                    return ":)";
                }
            }

            return (knowledgeRating[(int)numberOfRooms - 1]).ToString();
        }
    }
}
