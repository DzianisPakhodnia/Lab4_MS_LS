using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab4_MS_LS
{
    class CMS_LS
    {
        public int countOperations = 0;
        public int countTypes = 0;
        public int[][] arrayTypes;
        public int[] countProcessorsByTypes;
        public int[] TimeCalculationByTypes;
        public int[][] arrayH;
        public List<List<int>> List_chains = new List<List<int>>();
        public List<List<int>> steps = new List<List<int>>();      
        public List<List<List<int>>> List_operations_on_steps = new List<List<List<int>>>();

        private List<List<bool>> List_chains_ready_for_step = new List<List<bool>>();
        private List<List<int>> Duration_calculation_by_type = new List<List<int>>();

        public CMS_LS() { }

        public void Clear_Object()
        {
            countOperations = 0;
            countTypes = 0;
            arrayH = null;
            arrayTypes = null;
            countProcessorsByTypes = null;
            List_chains.Clear();
            List_chains_ready_for_step.Clear();
            steps.Clear();
            List_operations_on_steps.Clear();

        }
        private string Find_value(string s)
        {
            int index = s.IndexOf(':') + 2;            
            return s.Substring(index);   // извлечение подстроки с указанной позиции и до конца            
        }

        private int[] GetArray(string s)
        {
            
            string[] nums = s.Split(' ');
            int[] arr = new int[nums.Length];
            for (int i = 0; i < nums.Length; i++)
            {
               arr[i] = int.Parse(nums[i]);
            }
                       
            return arr;
        }

        

        public void File_Load(string filestr)
        {
            // получение данных из файла

            StreamReader file = new StreamReader(filestr);
            //step 1 чтение кол-ва операций
            string buff = "";
            buff = file.ReadLine();

            buff = Find_value(buff);            
            countOperations =  Convert.ToInt32(buff);

            //step 2 чтение кол-ва типов операций
            buff = "";
            buff = file.ReadLine();
            buff = Find_value(buff);
            countTypes = Convert.ToInt32(buff);

            //step 3 чтение операций каждого типа
            arrayTypes = new int[countTypes][];            
            for (int i = 0; i < countTypes; i++)
            {
                buff = "";
                buff = file.ReadLine();
                buff = Find_value(buff);              
                arrayTypes[i] = GetArray(buff);
            }

            //step 4 чтение количества процессоров каждого типа
            countProcessorsByTypes = new int[countTypes];
            for (int i = 0; i < countTypes; i++)
            {
                buff = "";
                buff = file.ReadLine();
                buff = Find_value(buff);
                countProcessorsByTypes[i] = Convert.ToInt32(buff);
            }


            //step 5 чтение времени выполнения операции каждого типа
            TimeCalculationByTypes = new int[countTypes];
            for (int i = 0; i < countTypes; i++)
            {
                buff = "";
                buff = file.ReadLine();
                buff = Find_value(buff);
                TimeCalculationByTypes[i] = Convert.ToInt32(buff);
            }

            //step 6 чтение таблицы смежности
            buff = "";
            buff = file.ReadLine();          

            arrayH = new int[countOperations][];

            for (int i = 0; i < countOperations; i++)
            {
                buff = "";
                buff = file.ReadLine();                
                arrayH[i] = GetArray(buff);
            }

            file.Close();                
        }

        public void Planning()
        {
            GetChains();
            List_chains_ready_for_step = Create_clone_chains_ready();
            WriteSteps();            
        }

        private List<List<bool>> Create_clone_chains_ready()
        {
            List<List<bool>> ready = new List<List<bool>>();
            for (int i = 0; i < List_chains.Count; i++)
            {
                ready.Add(new List<bool>());

                for (int k = 0; k < List_chains[i].Count; k++)
                {
                    ready[i].Add(false);
                    
                }
            }

            return ready;
        }
        private void GetChains()
        {
            // построить цепи зависимостей
            List<int> List_use = new List<int>();

            List<int> Chain = new List<int>();
            int operation = 0;

            while (List_use.Count < countOperations)
            {
                operation = 0;
                while (List_use.Contains(operation))
                {
                    operation++;
                }

                Chain.Clear();
                Chain.Add(operation);
                List_use.Add(operation);
                while (true)
                {
                    // найти в строке таблицы смежности следующее направление
                    bool index = false;
                    for (int i = 0; i < arrayH[operation].Length; i++)
                    {
                        if (arrayH[operation][i] == 1)
                        {
                            index = true;
                            operation = i;
                            break;
                        }
                    }

                    if (index) // если найдено следующее значение в цепи
                    {
                        Chain.Add(operation);
                        if (!List_use.Contains(operation))
                        {
                            List_use.Add(operation);
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                List_chains.Add(new List<int>(Chain));
            }

        }

             
        private void WriteSteps()
        {
            // переменная для инициализации List_operations_on_steps
            List<List<int>> ret = new List<List<int>>();             
            ret.Add(new List<int>()); // выполняющиеся
            ret.Add(new List<int>()); // не распределенные на предыдущем шаге
            ret.Add(new List<int>()); // готовые к распределению

            Duration_calculation_by_type.Add(new List<int>()); // операции
            Duration_calculation_by_type.Add(new List<int>()); // оставшаяся продолжительность выполнения

            // step zero
            
            int index_step = 0;
            steps.Add(new List<int>());           
            List_operations_on_steps.Add(ret);
            // добавление всех готовых к распледелению в правую часть списка
            for (int i = 0; i < List_chains.Count; i++)
            {
                List_operations_on_steps[index_step][2].Add(List_chains[i][0]);
            }
                      
           
          
            bool exist_nonplanOperation = false;
            // 1 и более шаги
            while (true)
            {
                exist_nonplanOperation = false;
                // проверка на наличие нераспланированных шагов
                for (int i = 0; i < List_chains_ready_for_step.Count; i++)
                {
                    if (List_chains_ready_for_step[i].Contains(false))
                    {
                        exist_nonplanOperation = true;
                        break;
                    }
                }
                // если нераспланированных шагов не осталось, то выйти
                if (!exist_nonplanOperation)
                {
                    break;
                }


                ///////////////////////////
                // добавление нового шага
                steps.Add(new List<int>());
                index_step++;

                // список всех операциий, которые готовы к планированию
                List<int> ready_operations = new List<int>();                
                for (int k = 0; k < 3; k++)
                {
                    for (int i = 0; i < List_operations_on_steps[index_step - 1][k].Count; i++)
                    {
                        ready_operations.Add(List_operations_on_steps[index_step - 1][k][i]);
                    }
                }

              
                // планировка операций на текущий шаг
                for (int i = 0; i < countTypes; i++)
                {
                   List<int> res = GetOperationsByType(i, ready_operations);
                   foreach (int a in res)
                   {
                       steps[index_step].Add(a);
                   }
                }

                // уменьшение продолжительности вычисления операций
                for (int i = 0; i < Duration_calculation_by_type[0].Count; i++)
                {
                    Duration_calculation_by_type[1][i]--;
                }

                List<List<int>> ret_2 = new List<List<int>>();
                ret_2.Add(new List<int>());
                ret_2.Add(new List<int>());
                ret_2.Add(new List<int>());
                List_operations_on_steps.Add(ret_2); // добавление пустого объекта

                // перемещение всех нераспланированных на этом шаге операций в правую часть списка
                List_operations_on_steps[index_step][1] = minus(ready_operations, steps[index_step]);
               
                // изменение на true всех операций, которые распределены на текущем шаге
                for (int i = 0; i < steps[index_step].Count; i++)
                {
                    if (Duration_calculation_by_type[1][i] == 0)
                    {
                        for (int k = 0; k < List_chains_ready_for_step.Count; k++)
                        {
                            if (List_chains[k].Contains(Duration_calculation_by_type[0][i]))
                            {
                                int index_operation = List_chains[k].IndexOf(Duration_calculation_by_type[0][i]);
                                List_chains_ready_for_step[k][index_operation] = true;
                            }
                        }                        
                    }
                }

                for (int i = 0; i < Duration_calculation_by_type[0].Count; i++)
                {
                    if (Duration_calculation_by_type[1][i] == 0)
                    {
                        Duration_calculation_by_type[0].RemoveAt(i);
                        Duration_calculation_by_type[1].RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        List_operations_on_steps[index_step][0].Add(Duration_calculation_by_type[0][i]);
                    }
                }

                // поиск всех операций, готовых к распределению
                int index_false = 0;
                int value_index_false = -1;
                for (int i = 0; i < List_chains_ready_for_step.Count; i++)
                {
                    // поиск индекса в цепи, где значение = false
                    index_false = Find_index(List_chains_ready_for_step[i]);                    
                    // если операция не содержится в левой части списка,
                    // то добавить в правую часть списка
                    if (index_false != -1)
                    {
                        value_index_false = List_chains[i][index_false];
                        if (!List_operations_on_steps[index_step][0].Contains(value_index_false) &&
                            !List_operations_on_steps[index_step][1].Contains(value_index_false) &&
                            !List_operations_on_steps[index_step][2].Contains(value_index_false) &&
                            Check_predecessors(value_index_false))
                        {
                            List_operations_on_steps[index_step][2].Add(value_index_false);
                        }
                    }
                }                
            }
        }

        List<int> minus(List<int> a, List<int> b)
        {
            List<int> res = new List<int>();
            for (int i = 0; i < a.Count; i++)
            {
                if (!b.Contains(a[i]))
                {
                    res.Add(a[i]);
                }
            }
            return res;
        }

        class Priority_Operation
        {
            public int priority;
            public int operation;

            public Priority_Operation(int priority, int operation)
            {
                this.priority = priority;
                this.operation = operation;
            }
        }

        // класс-компаратор по убыванию
         class CompInv<T> : IComparer<T>
        where T : Priority_Operation
    {
        // Реализуем интерфейс IComparer<T>
        public int Compare(T x, T y)
        {
            if (x.priority < y.priority)
                return 1;
            if (x.priority > y.priority)
                return -1;
            else return 0;
        }
    }

        private List<int> GetOperationsByType(int type, List<int> Ready)
        {
            List<int> on_step = new List<int>();
            List<int> Ready_2 = new List<int>();
            for (int i = 0; i < Ready.Count; i++)
            {
                // если операция в списке не того типа, то удалить из списка
                if (arrayTypes[type].Contains((Ready[i]) + 1))
                {
                    Ready_2.Add(Ready[i]);
                }
            }

            Ready = Ready_2;
            // определить приоритет каждой операции по длине оставшейся цепи
            List<Priority_Operation> List_priority_operations = new List<Priority_Operation>();

            for (int i = 0; i < Ready.Count; i++)
            {
                List_priority_operations.Add(new Priority_Operation(MaxLengthChain(Ready[i]), Ready[i]));
            }

            // сортировка по убыванию
            CompInv<Priority_Operation> cp = new CompInv<Priority_Operation>();
            List_priority_operations.Sort(cp);

            try
            {
                for (int i = 0; i < countProcessorsByTypes[type]; i++)
                {
                    if (!Duration_calculation_by_type[0].Contains(List_priority_operations[i].operation))
                    {
                        on_step.Add(List_priority_operations[i].operation);
                        Duration_calculation_by_type[0].Add(List_priority_operations[i].operation);
                        Duration_calculation_by_type[1].Add(TimeCalculationByTypes[type]);
                    }
                    else 
                    {
                        on_step.Add(List_priority_operations[i].operation);
                    }
                }
            }
            catch (IndexOutOfRangeException) // словить, если число процессоров больше, чем доступных операций
            {
                return on_step;
            }
            catch (ArgumentOutOfRangeException) // словить, если число процессоров больше, чем доступных операций
            {
                return on_step;
            }


            return on_step;
        }

        public int MaxLengthChain(int operation)
        {
            int max = 0;
            for (int i = 0; i < List_chains.Count; i++) // пройтись по всем цепям
            {
                if (List_chains[i].Contains(operation)) // если цепь содержит операцию
                {
                    // длина от операции до конца цепи
                    int len = List_chains[i].Count - (List_chains[i].IndexOf(operation) + 1); 

                    if (max < len)
                        max = len;
                }
            }
            return max;
        }

        private bool Check_predecessors(int operation)
        {
            //проверить всех предшественников, все ли они распранированы
            List<bool> predesessors = new List<bool>();
            bool check = false;
            for (int i = 0; i < List_chains.Count; i++)
            {
                int index = Find_index(List_chains[i], operation);
                index--;
                if (index >= 0)
                {
                    predesessors.Add(List_chains_ready_for_step[i][index]);
                }
            }
            // проверить в списке на наличие хоть одного нераспланированного
            check = !predesessors.Contains(false);
            return check;
        }

        private int Find_index(List<bool> L)
        {
            int index = -1;
            for (int i = 0; i < L.Count; i++)
            {
                if (L[i] == false)
                {
                    index = i;
                    return index;
                }
            }

            return index;
        }

        private int Find_index(List<int> L, int operation)
        {
            int index = -1;
            for (int i = 0; i < L.Count; i++)
            {
                if (L[i] == operation)
                {
                    index = i;
                    return index;
                }
            }

            return index;
        }
    }
}
