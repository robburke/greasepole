using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverLegend.Util
{
    public class EventArgs<T> : EventArgs
    {
        public EventArgs(T value) { _Value = value; }

        public T Value { get { return _Value; } }
        private T _Value;
    }

    public class GameLoop
    {
        public static int RenderFps = 24;

        FrameworkElement m_TargetElement = null;
        Storyboard m_Storyboard;
        DateTime m_LastUpdateTime = DateTime.MinValue;
        TimeSpan m_ElapsedTime;

        public event EventHandler<EventArgs<TimeSpan>> Updated;

        public void Attach(FrameworkElement fe)
        {
            m_TargetElement = fe;
            m_Storyboard = new Storyboard();
            m_Storyboard.Duration = new Duration(TimeSpan.FromMilliseconds(1000/(double)RenderFps));
            
            fe.Resources.Add("gameloopStoryboard", m_Storyboard);
            m_LastUpdateTime = DateTime.Now;
            m_Storyboard.Completed += StoryboardCompleted;
            m_Storyboard.Begin();
        }

        public void Detach(FrameworkElement fe)
        {
            m_Storyboard.Stop();
            fe.Resources.Remove("gameloopStoryboard");
        }

        void StoryboardCompleted(object sender, EventArgs e)
        {
            m_ElapsedTime = DateTime.Now - m_LastUpdateTime;
            m_LastUpdateTime = DateTime.Now;
            if (Updated!=null) Updated(this, new EventArgs<TimeSpan>(m_ElapsedTime));
            if (!Disposed) m_Storyboard.Begin();
        }

        public void Dispose()
        {
            Disposed = true;
        }
        bool Disposed = false;
    }
}
