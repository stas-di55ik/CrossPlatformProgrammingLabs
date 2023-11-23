using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;

namespace CPP_Lab3
{
    public class BrainLand
    {
        [Option(ShortName = "i")]
        public string InputFile { get; }

        [Option(ShortName = "o")]
        public string OutputFile { get; }

        StreamReader inputReader;
        StreamWriter outputWriter;

        static long Infinity = 100000000000; // Максимальне значення для нескінченності
        static List<List<Tuple<long, long>>> roomConnections = new List<List<Tuple<long, long>>>(2000);
        static List<Edge> doors = new List<Edge>();
        static List<long> knowledgeRating = new List<long>(2000);
        static List<long> previousRoom = new List<long>(2000);
        static List<long> usedRooms = new List<long>(2000);
        static List<long> blockedRooms = new List<long>(2000);

        struct Edge
        {
            public long from, to, knowledgeChange;
        }

        // Рекурсивна функція для позначення пройдених кімнат
        static void MarkVisitedRooms(long currentRoom, long groupNumber)
        {
            usedRooms[(int)currentRoom] = groupNumber;
            foreach (var connection in roomConnections[(int)currentRoom])
            {
                if (usedRooms[(int)connection.Item1] == 0)
                    MarkVisitedRooms(connection.Item1, groupNumber);
            }
        }

        public static void Main(string[] args)
            => CommandLineApplication.Execute<BrainLand>(args);

        private void OnExecute()
        {
            inputReader = new StreamReader(InputFile);
            outputWriter = new StreamWriter(OutputFile);

            long numberOfRooms, numberOfDoors;
            string[] roomAndDoorCounts = inputReader.ReadLine().Split();
            numberOfRooms = long.Parse(roomAndDoorCounts[0]);
            numberOfDoors = long.Parse(roomAndDoorCounts[1]);

            // Ініціалізація списків для зберігання даних про кімнати, двері та інші дані
            for (int i = 0; i < 2000; i++)
            {
                roomConnections.Add(new List<Tuple<long, long>>());
                knowledgeRating.Add(-Infinity);
                previousRoom.Add(-1);
                usedRooms.Add(0);
                blockedRooms.Add(0);
            }

            // Зчитування даних про двері та їх рейтинг знань
            for (int i = 0; i < numberOfDoors; i++)
            {
                string[] doorInfo = inputReader.ReadLine().Split();
                Edge door = new Edge();
                door.from = long.Parse(doorInfo[0]);
                door.to = long.Parse(doorInfo[1]);
                door.knowledgeChange = long.Parse(doorInfo[2]);
                door.from--; door.to--; // Зменшуємо на 1, оскільки індексація з 0
                roomConnections[(int)door.to].Add(new Tuple<long, long>(door.from, door.knowledgeChange));
                doors.Add(door);
            }

            knowledgeRating[0] = 0; // Початкова кімната має 0 знань

            // Знаходження найкращого способу пройти лабіринт
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
                outputWriter.Write(":("); // Лабіринт непрохідний
                outputWriter.Close();
                return;
            }

            // Позначаємо пройдені кімнати та блокуємо цикли
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
                    outputWriter.Write(":)"); // Можна набрати необмежено багато знань
                    outputWriter.Close();
                    return;
                }
            }

            outputWriter.Write(knowledgeRating[(int)numberOfRooms - 1]); // Максимальна кількість набраних знань
            outputWriter.Close();
        }
    }
}