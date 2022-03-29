using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using NewXXSY.Contrl;
using NewXXSY.Droid;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(PostWebView), typeof(PostWebViewRenderer))]
namespace NewXXSY.Droid
{
    public class PostWebViewRenderer : WebViewRenderer
    {
		private float x;
		private float y;

		private bool sweping;

		private bool check = true;
		public PostWebViewRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);
			
        }



        public override bool DispatchTouchEvent(MotionEvent e)
        {
			MotionEventActions action = e.Action;
			switch ((int)action)
			{
				case 1:
				case 3:
					sweping = false;
					check = true;
					break;
				case 0:
					x = e.GetX();
					y = e.GetY();
					break;
				case 2:
					{
						float nowX = e.GetX() - x;
						float nowY = e.GetY() - y;
						if (check && (nowX != 0f || nowY != 0f))
						{
							check = false;
							sweping = Math.Abs(nowY) * 3f > Math.Abs(nowX);
						}
						break;
					}
			}
		((Android.Views.View)this).Parent.RequestDisallowInterceptTouchEvent(sweping);
			return base.DispatchTouchEvent(e);
		}
    }

}