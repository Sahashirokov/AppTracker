   M S B A M L   `   `   `  ����E  AAppTracker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:LauncherApp.MVVM.View.PagesLauncherApp.MVVM.View.Pages  5      F BCalcBinding, Version=2.5.2.0, Culture=neutral, PublicKeyToken=null>.clr-namespace:CalcBinding;assembly=CalcBindingCalcBinding 5      .    (LauncherApp.MVVM.View.Pages.FavoritePage   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ;local)clr-namespace:LauncherApp.MVVM.View.Pages     5      <c.clr-namespace:CalcBinding;assembly=CalcBinding     5        N� Title$  Favorite��5	       � DataContext 5
      �� *	VmAppList+   ViewModelLocator �� Source# [  .��5      � .+�5   
   ��   ModernLoaderStyle#��[ 6   
�������y�5      �� *	IsLoading+  BoolToVisibilityConverter �� 	Converter# [ 5      �� 5   
   $
��Auto=�5      
������ ��5      �� *AppM+��5      �� 5       �� 
TargetType" ��6   .�5      ��  �� Property � �� �� Value$ Stretch=�6C   �� 5       � ��$ 6q�60   6<   5      ��5      �� 5      .R�5      � ���#24262D � ColumnDefinitions 5      �� 5      $
��Auto �6/   �� 5       $
��Auto �6/   �� 5!      $
��Auto �6/   �� 5"      $
��Auto �6/   �� 5#      $
��Auto �6/   �� 5$      $
��Auto �6/   �� 5%      $
��Auto �6/   �� 5&      $
��Auto �6/   �� 5'      $��* �6/   .+�5*      �� $��0��6    $��42��6H   $��42��6S   $��10 0 0 0q�6_   ��60   �� *Icon+6_   �� 5+       ��Name$ StartOrStop��5,   !   $��1��5+   !     CommonStartStyle#��[ 50   !   ��5-   !   �� *	IsRunning+  BoolToStartTextConverter# [ ��5.   !   �� *DataContext.ToggleAppCommand+	 �� RelativeSource	 �� 
 �� Mode$
 FindAncestor=� �� AncestorType ListBox��5/   !   �� 50   !   �� 5:      $��3��6!   $��Удалить��5;   !     CommonDeleteStyle#��[ 5=   !   ��5;   3   �� *DataContext.RemoveApp+	 �� $
 FindAncestor=� ListBox��5<   !   �� 5=   !   �� 5@      $��4��6$    �� Padding$ 2px 10px 8px 2pxq�65   �� 5A      $�Название:��6   $��Arial)�5B      $��16(�5C      $
��Bold"�5D      
�������5E       �� 5F       �� TextDecorations$  {�5G      $��Segoe UI)�5H      $��14(�5I      
��������5F      �� *Name+5J      5K      �� 5L      $��5��6$   $ 2px 10px 8px 2pxq�64   �� 5M      $!�Первый запуск: ��6   $��Arial)�5N      $��16(�5O      $
��Bold"�5P      
�������5Q       �� 5R      $��Segoe UI)�5S      $��14(�5T      
��������5R      �� *	StartTime+ �� Mode$ OneWay=� �� StringFormat$ {0:HH\:mm\:ss}��5U      5V      �� 5W      $��6��6$   $ 2px 10px 8px 2pxq�64   �� 5X      $��Arial)�5[      $��16(�5\      $
��Bold"�5]      �5X        CalcBinding.Binding  *64IsRunning ?  'Активно': 'Не активно'+   Mode$ OneWay=���5^        * IsRunning ? "#51FF44" : "Gray"+5_      �� 5`      $��7��6   $��Segoe UI)�5c   $   $��14(�5d   $   
�������$ 0px 10px 8px 0pxq�5g      ��5a        *
Duration+$ OneWay=�   StringFormat$ {0:hh\:mm\:ss}��y�5f   $     *	IsRunning+   FalseToVisibility$ 	Collapsed=�5h      �� 5i      $��8��6$   $ 0px 10px 8px 0pxq�64   �� 5j      $!�Всего времени: ��6   $��Arial)�5k      $��16(�5l      $
��Bold"�5m      
�������5n       �� 5o      $��Segoe UI)�5p      $��14(�5q      
��������5o      �� *	TotalTime+$ {0:hh\:mm\:ss}��5r      5s      5t      5u      ��5x      �� 5y       �� 
TargetType" ��6"   .R�5z      �� ._�5{      � .+�5|      ��  ��Name$ ScrollViewer��6+   .��5}   "   �� 5~      �� 5      $<��6У вас нет выбранных элементов��6(   $��Arial)�5�   (   $��18(�5�   (   $
��Bold"�5�   (   
������� �� TextAlignment$ Center=�5�   (   $��Center=�5�   (   y�5�   "   `� 5�   &      AllTrueToVisibilityConverter `� 	Converter# [ 63   .1�5�   *   ��  �� Path$ 	IsLoading�62     InvertBoolConverter# [ 5�   2   �� 5�   *   $ Items.IsEmpty�62   	 5�   2   ��  ListBox5�   '   5�      5�      5�      5�      5�      5�      5�      