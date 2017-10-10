using Maze.Library;
using System.Collections.Generic;

namespace Maze.Solver {
    public class RobotController {

        private IRobot robot;

        private ISet<(int x, int y)> visited = new HashSet<(int x, int y)>();
        private bool reachedEnd = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotController"/> class
        /// </summary>
        /// <param name="robot">Robot that is controlled</param>
        public RobotController(IRobot robot) {
            // Store robot for later use
            this.robot = robot;
        }

        /// <summary>
        /// Moves the robot to the exit
        /// </summary>
        /// <remarks>
        /// This function uses methods of the robot that was passed into this class'
        /// constructor. It has to move the robot until the robot's event
        /// <see cref="IRobot.ReachedExit"/> is fired. If the algorithm finds out that
        /// the exit is not reachable, it has to call <see cref="IRobot.HaltAndCatchFire"/>
        /// and exit.
        /// </remarks>
        public void MoveRobotToExit() {
            robot.ReachedExit += (_, __) => reachedEnd = true;

            moveOut();
            if (!reachedEnd) robot.HaltAndCatchFire();
        }


        private void moveOut(int x=0, int y=0) {
            if (!reachedEnd && visited.Add((x, y))) {

                if (robot.TryMove(Direction.Right)) {
                    moveOut(x + 1, y);
                    if (!reachedEnd)
                        robot.Move(Direction.Left);
                }
                if (!reachedEnd && robot.TryMove(Direction.Left)) {
                    moveOut(x - 1, y);
                    if (!reachedEnd)
                        robot.Move(Direction.Right);
                }
                if (!reachedEnd && robot.TryMove(Direction.Up)) {
                    moveOut(x, y - 1);
                    if (!reachedEnd)
                        robot.Move(Direction.Down);
                }
                if (!reachedEnd && robot.TryMove(Direction.Down)) {
                    moveOut(x, y + 1);
                    if (!reachedEnd)
                        robot.Move(Direction.Up);
                }
            }
        }
    }
}
