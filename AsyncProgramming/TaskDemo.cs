using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.AsyncProgramming
{
    internal class TaskDemo
    {

        public void Run2()
        {
            Console.WriteLine($"start {(DateTime.Now.Ticks % (long)1e8) / 1e7}");
            Task<String> taskCoffee = Task.Run(MakeCoffee);
            Task<String> taskBacon = Task.Run(RoastBacon);
            Task<String> taskToast = Task.Run(MakeToast);
            Console.WriteLine(taskToast.Result);
            Console.WriteLine(taskBacon.Result);
            Console.WriteLine(taskCoffee.Result);
        }

        private String MakeToast()
        {
            Task.Delay(100).Wait();
            return $"Toast ready {(DateTime.Now.Ticks % (long)1e8) / 1e7}";
        }

        private String RoastBacon()
        {
            Task.Delay(300).Wait();
            return $"Bacon ready {(DateTime.Now.Ticks % (long)1e8) / 1e7}";
        }

        private String MakeCoffee()
        {
            Task.Delay(1000).Wait();
            return $"Coffee ready {(DateTime.Now.Ticks % (long)1e8) / 1e7}";
        }

        public void Run()
        {
            Console.WriteLine("TaskDemo start");
            Task task = Task.Run(TaskAction);
            //Task<String> taskString = Task.Run(TaskStringAction);
            Task<String> taskString = Task.Run(() => TaskStringAction(2000));
            Task.Delay(500).Wait();
            Console.WriteLine("TaskDemo Wait finish");
            task.Wait();

            Console.WriteLine($"taskString.Result = {taskString.Result}");

            var action = () => TaskStringAction(-1);
            try
            {
                Task.Run(action);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Task.Delay(100).Wait();

            try
            {
                Task.Run(() => TaskStringAction(-1)).Wait();
                Console.WriteLine(action);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("TaskDemo finish");
        }

        private void TaskAction()
        {
            Console.WriteLine("TaskAction start");
            Task.Delay(1000).Wait();
            Console.WriteLine("TaskAction finish");
        }

        private String TaskStringAction(int timeout)
        {
            if (timeout < 0)
            {
                throw new ArgumentOutOfRangeException("timeout must be positive");
            }
            Console.WriteLine($"TaskStringAction {timeout} start");
            Task.Delay(timeout).Wait();
            Console.WriteLine($"TaskStringAction {timeout} finish");
            return "TaskStringAction";
        }
    } 
}
