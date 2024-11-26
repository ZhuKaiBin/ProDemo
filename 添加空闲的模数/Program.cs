namespace 添加空闲的模数
{
    internal class Program
    {
        static void Main(string[] args)
        {

            {
                var s = 8 % 3;


            }
            // 初始化数据
            List<DataRecord> dataRecords = new List<DataRecord>() {
                                             new DataRecord(){Id=1, Modular="24", SortOrder=1},
                                             new DataRecord(){Id=2, Modular="6/2", SortOrder=2},
                                             new DataRecord(){Id=3, Modular="6/2", SortOrder=3},
                                             new DataRecord(){Id=4, Modular="8/3", SortOrder=4},
                                             new DataRecord(){Id=5, Modular="12", SortOrder=5},
                                             new DataRecord(){Id=6, Modular="12", SortOrder=6},
                                         };


            //应该先排序，然后再把后面的再更新SortOrder
            // 要新增的 Modular
            var newAddModular = "8/3";
            if (newAddModular.Contains("/"))
            {
                //先获取到数据中有多个个相同的记录，
                //然后再看是按照几个进行分组的

                var existeds = dataRecords.Where(x => x.Modular == newAddModular).OrderBy(x => x.SortOrder).ToList();
                var fenMu = Convert.ToInt32(newAddModular.Split('/')[1]);
                var sd = existeds.Count % fenMu;//除法中取模，要是取模不为0，那就是还有空位
                if (sd != 0)
                {

                    //如果不等于0,那么就是要变更SortOrder
                    var 当前的最大模数的Sort = dataRecords.OrderBy(x => x.SortOrder).ToList().Where(x => x.Modular == newAddModular).FirstOrDefault();

                    //var newDataRecord = new DataRecord()
                    //{
                    //    SortOrder = 当前的最大模数的Sort + 1,
                    //    Modular = newAddModular,
                    //};


                    //var dd = dataRecords.Where(x => x.SortOrder > 当前的最大模数的Sort.SortOrder).ToList();
                    //dd的所有的sortOrder都加1;


                }
                else//如果等于0的话，那么就是新增了
                {

                    //取出来最大值的SortOrder，然后新增
                }



            }
            else
            {
                //如果是整数的话，那么就直接找到最大值的sortOrder，就可以排序了



            }

        }
    }



    public class DataRecord
    {
        public int Id { get; set; }
        public string Modular { get; set; }
        public int SortOrder { get; set; }
    }


    public class TestModular
    {
        public int funcUnitId { set; get; }

        public string? modular { set; get; }

        public int sortOrder { set; get; }

        public bool isFull = false;

    }
}
