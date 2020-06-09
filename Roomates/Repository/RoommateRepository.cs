using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;
using Roommates.Repositories;

namespace Roommates.Repository
{
    class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }
        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                // Note, we must Open() the connection, the "using" block doesn't do that for us.
                conn.Open();

                // We must "use" commands too.
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Here we setup the command with the SQL we want to execute before we execute it.
                    cmd.CommandText = "SELECT Id, FirstName, LastName, RentPortion, MoveInDate, RoomId FROM Roommate";

                    // Execute the SQL in the database and get a "reader" that will give us access to the data.
                    SqlDataReader reader = cmd.ExecuteReader();

                    // A list to hold the rooms we retrieve from the database.
                    List<Roommate> roommates = new List<Roommate>();

                    // Read() will return true if there's more data to read
                    while (reader.Read())
                    {
                        // The "ordinal" is the numeric position of the column in the query results.
                        //  For our query, "Id" has an ordinal value of 0 and "Name" is 1.
                        int idColumnPosition = reader.GetOrdinal("Id");

                        // We user the reader's GetXXX methods to get the value for a particular ordinal.
                        int idValue = reader.GetInt32(idColumnPosition);
                        string firstNameValue = reader.GetString(reader.GetOrdinal("FirstName"));
                        string lastNameValue = reader.GetString(reader.GetOrdinal("LastName"));
                        int rentPortionValue = reader.GetInt32(reader.GetOrdinal("RentPortion"));
                        DateTime dateTime = reader.GetDateTime(reader.GetOrdinal("MoveInDate"));
                        int roomIdValue = reader.GetInt32(reader.GetOrdinal("RoomId"));

                        // Now let's create a new roommate object using the data from the database.
                        Roommate roommate = new Roommate
                        {
                            Id = idValue,
                            Firstname = firstNameValue,
                            Lastname = lastNameValue,
                            RentPortion = rentPortionValue,
                            MovedInDate = dateTime,
                            Room = null
                        };

                        // ...and add that room object to our list.
                        roommates.Add(roommate);
                    }

                    // We should Close() the reader. Unfortunately, a "using" block won't work here.
                    reader.Close();

                    // Return the list of rooms who whomever called this method.
                    return roommates;
                }
            }
        }

        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT FirstName, LastName, RentPortion, MoveInDate, RoomId FROM Room WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;

                    // If we only expect a single row back from the database, we don't need a while loop.
                    if (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = id,
                            Firstname = reader.GetString(reader.GetOrdinal("FirstName")),
                            Lastname = reader.GetString(reader.GetOrdinal("LastName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MovedInDate= reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                    };
                    }

                    reader.Close();

                    return roommate;
                }
            }
        }

        public List<Roommate> GetAllWithRoom(int roomId)
        {

            using (SqlConnection conn = Connection)
            { 
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Here we setup the command with the SQL we want to execute before we execute it.
                    cmd.CommandText = @"SELECT r.Id, r.FirstName, r.LastName, r.RentPortion, r.MoveInDate, rm.Name, rm.MaxOccupancy FROM Roommate r JOIN ROOM rm ON rm.Id = r.roomId WHERE roomId = @roomId";
                     cmd.Parameters.AddWithValue("@roomId", roomId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    // A list to hold the rooms we retrieve from the database.
                    List<Roommate> roommates = new List<Roommate>();

                    // Read() will return true if there's more data to read
                    while (reader.Read())
                    {
                        // The "ordinal" is the numeric position of the column in the query results.
                        //  For our query, "Id" has an ordinal value of 0 and "Name" is 1.
                        int idColumnPosition = reader.GetOrdinal("Id");

                        // We user the reader's GetXXX methods to get the value for a particular ordinal.
                        int idValue = reader.GetInt32(idColumnPosition);
                        string firstNameValue = reader.GetString(reader.GetOrdinal("FirstName"));
                        string lastNameValue = reader.GetString(reader.GetOrdinal("LastName"));
                        int rentPortionValue = reader.GetInt32(reader.GetOrdinal("RentPortion"));
                        DateTime dateTime = reader.GetDateTime(reader.GetOrdinal("MoveInDate"));
                        string roomName = reader.GetString(reader.GetOrdinal("name"));
                        int maxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy"));
                        // Now let's create a new roommate object using the data from the database.
                        Roommate roommate = new Roommate
                        {
                            Id = idValue,
                            Firstname = firstNameValue,
                            Lastname = lastNameValue,
                            RentPortion = rentPortionValue,
                            MovedInDate = dateTime,
                            Room = new Room
                            {
                                Id= roomId,
                                Name = roomName,
                                MaxOccupancy = maxOccupancy
                            }
                        };

                        // ...and add that room object to our list.
                        roommates.Add(roommate);
                    }
                    reader.Close();
                    return roommates;
                }
            }
        }



    }
}
