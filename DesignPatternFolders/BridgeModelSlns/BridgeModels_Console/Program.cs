namespace BridgeModels_Console
{
    #region
    //桥接模式（Bridge Pattern）是一种结构型设计模式，目的是将抽象与实现解耦，使得它们可以独立变化。桥接模式主要包括两个角色：抽象角色和实现角色。
    //我有一个形状，然后可以搭配多个颜色，我这里就是把形状和颜色分开，形状是抽象角色，颜色是实现角色。
    #endregion
    internal class Program
    {
        static void Main(string[] args)
        {

            // 使用 DrawingAPI1 绘制
            Shape circle1 = new Circle(5, 10, 20, new DrawingAPIColor1());
            circle1.Draw();




            Shape circle2 = new Circle(5, 10, 20, new DrawingAPI2Color());
            circle2.Draw();
        }
    }



    /// <summary>
    /// 抽象角色（Abstraction）
    /// </summary>
    public abstract class Shape
    {
        protected IDrawingColorAPI drawingColorAPI;

        // 构造函数初始化实现部分
        protected Shape(IDrawingColorAPI drawingAPI)
        {
            this.drawingColorAPI = drawingAPI;
        }


        // 抽象的绘制方法，调用实现部分的具体方法
        public abstract void Draw();
        public abstract void Resize(float percentage);
    }



    public class Circle : Shape
    {
        private double x, y, radius;
        public Circle(double x, double y, double radius, IDrawingColorAPI drawingColorAPI) : base(drawingColorAPI)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
        }

        public override void Draw()
        {
            drawingColorAPI.DrawShapeColor(x, y, radius);
        }

        public override void Resize(float percentage)
        {
            radius *= percentage;
        }

    }


    #region

    #endregion

}
