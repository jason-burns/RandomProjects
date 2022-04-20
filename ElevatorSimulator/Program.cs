using System;

namespace ElevatorSimulator
{
    enum ElevatorType
    {
        Regular,
        HighSpeed,
    }

    class Building
    {
        public Elevator[] elevators;
        public ElevatorType elevatorType;

        public int floors;

        public Building(ElevatorType elevatorType, int elevatorCount, int floors)
        {
            elevators = new Elevator[elevatorCount];
            this.elevatorType = elevatorType;
            this.floors = floors;

            for (int i = 0; i < elevatorCount; i++)
            {
                elevators[i] = BuildElevator();
            }
        }

        private Elevator BuildElevator()
        {
            switch(elevatorType)
            {
                case ElevatorType.HighSpeed:
                    return Elevator.BuildHighSpeedElevator();
                default:
                    return Elevator.BuildElevator();
            }
        }
    }

    class Elevator
    {
        private int accelerationTime;
        private int speedTimePerFloor;
        private int currentFloor = 0;

        private int capacity;

        public int passengers;
        public bool inUse;

        public bool isOnFloor(int floor)
        {
            return currentFloor == floor;
        }


        public static Elevator BuildHighSpeedElevator()
        {
            return new Elevator
            {
                accelerationTime = 2,
                speedTimePerFloor = 2,
                capacity = 20,
                passengers = 0,
            };
        }

        public static Elevator BuildElevator()
        {
            return new Elevator
            {
                accelerationTime = 6,
                speedTimePerFloor = 4,
                capacity = 10,
                passengers = 0,
            };
        }
    }

    class Passenger
    {

    }

    class Program
    {
        static void Main(string[] args)
        {
            // use case #1: persons leaving apartment building in the morning.
            // 10% arrivals, 90% departures
            // bell curve usage
            // simulate two elevators taking people to ground level and returning up.
            // find ways of reducing the amount of wait time when many people are departing the building.

            // use case #2: persons coming and going from apartment building during the day.
            // 50% arrivals, 50% departures
            // steady usage

            // use case #3: persons arriving at office building in the morning.
            // 100% arrivals, 0% departures
            // bell curve usage

            // build an array of passengers that will queue for the elevator.
            // each passenger is on their own floor of the building
            // set a random time that they begin waiting, weighted towards the middle ( bell curve )
            // when they begin waiting, request the elevator.
            // when an elevator arrives, all guests will board if room is available
            // guests add their requested floor to the elevator queue

            // if an elevator is not in use and available: it begins travelling to their floor
            // if an elevator in use and higher than the floor it will stop at the floor
            // if an elevator is in use and lower than their floor, it will queue up to go back to the requested floor

            // variations:
            // if the elevator has no requests
            // leave the elevator at the bottom floor until it is requested
            // queue up the elevator to the middle floor
            // queue up the elevator to the top floor

            // elevators will go 1 floor up if 

            Building smallApartment = new Building(2);

            Building


            // time of day
            // morning traffic
            // midday traffic
            // evening traffic
            // night traffic

            // commuters - average wait time on ground floor to go up
            // commuters - average wait time on non-ground floor to go down
        }
    }
}
