using System;
using System.Collections.Generic;
using Roommates.Models;
using Roommates.Repositories;

namespace Roommates
{
    class Program
    {
        /// <summary>
        ///  This is the address of the database.
        ///  We define it here as a constant since it will never change.
        /// </summary>
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);

            Console.WriteLine("Getting All Rooms:");
            Console.WriteLine();

            List<Room> allRooms = roomRepo.GetAll();

            foreach (Room room in allRooms)
            {
                Console.WriteLine($"{room.Id} {room.Name} {room.MaxOccupancy}");
            }

            Console.WriteLine("----------------------------");
            Console.WriteLine("Getting Room with Id 1");

            Room singleRoom = roomRepo.GetById(1);

            Console.WriteLine($"{singleRoom.Id} {singleRoom.Name} {singleRoom.MaxOccupancy}");
            //adding bathroom
            //Room bathroom = new Room
            //{
            //    Name = "Bathroom",
            //    MaxOccupancy = 1
            //};

            //roomRepo.Insert(bathroom);

            //Room shed = new Room
            //{
            //    Name = "Shed",
            //    MaxOccupancy = 50
            //};
            //roomRepo.Insert(shed);

            Console.WriteLine("-------------------------------");
            //Console.WriteLine($"Added the new Room with id {shed.Id} and name of {shed.Name}");
            //bathroom.MaxOccupancy = 18;
            //roomRepo.Update(bathroom);

            //Console.WriteLine($" bathrooms occupancy is {bathroom.Id}");

            roomRepo.Delete(9);
            roomRepo.GetAll();

        }
    }
}