   M S B A M L   `   `   `  ����E  AAppTracker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:LauncherApp.MVVM.View.PagesLauncherApp.MVVM.View.Pages  5      F BCalcBinding, Version=2.5.2.0, Culture=neutral, PublicKeyToken=null>.clr-namespace:CalcBinding;assembly=CalcBindingCalcBinding 5      -    'LauncherApp.MVVM.View.Pages.AllAppsPage   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ;local)clr-namespace:LauncherApp.MVVM.View.Pages     5      <c.clr-namespace:CalcBinding;assembly=CalcBinding     5        N� Title$  AllAppsPage��5	       � DataContext 5
      �� *AllAppsViewModel+   ViewModelLocator �� Source# [  .��5      � ���#010B2F.+�5   
   �  � RowDefinitions 5      �� 5      $��68 �6    6.   �� 5      $��* �6    .+�5      � $��0��6   $��68��6    ���#46348E.+�5      �� $$��Выбрать в ручную��6     AddInPathButton#��[ 64   $	��164��5      $��36��6%   $��Center=�61   $
��left=�6L   $��20 0 0 0q�5      ��5      �� *ManualAddToFavoriteCommand+5      �� 5      $��240px��5      $
��40px��6(   $��Center=�66   $
��Left=�5      $��	200 5 5 5q�65   $��0 0 0 0q�6H     ModernTextBox#��[ 5      ��5      �� *	SearchApp+ �� UpdateSourceTrigger$ PropertyChanged=�.��5      5      �� 5      $��Автозапуск��6D   $	��Top=�5      $��Right=�63   $��20 20 20 20q�5       $��16(�5!      $��Arial)�5"      $��Center=�5#      
������� d� 	IsChecked 5      �� *IsAutoStartEnabled+5$      5&      �� 5'   
   $��1��6   $��Stretch=�5(       %� IsVirtualizing$
 True��5)       %� VirtualizationMode$ 	Recycling=�5*      $
��True��5+      ���#040E27$��0q�5,   (   ��5'       �� *Applications+��5-      �� 5.      .R�5/      ��  �� Orientation$ 
Horizontal=�6!   $��5q�6:   ���#24262D.+�50      �� $��42��6=   $��42��6H   $��10 0 0 0q�6T   ��6    �� *	Info.Icon+6T   �� 51      $��10,0,2,2q�6%   $ Vertical=�67   $��Center=�6N   .+�52      �� $ 
Horizontal=�6)   .+�53   "   �� $,��&Добавить в избранное��6)     AddFavoriteApp#��[ 54   )   ��55   )   �� *" DataContext.AddToFavoriteCommand+	 �� RelativeSource	 �� 
 �� AncestorType
 ListView��57   )   �� y�58   )   �� *
IsFavorite+  BoolToVisibilityConverter �� 	Converter# [  �� ConverterParameter$ Reverse���� 5;   "   $��В избранном��6,   
��� � �$��Center=�5<   ?   $
��Aria)�5=   ,   $��16(�5>   ,   $��5q�5?   ,   y�5@   ,   �� *
IsFavorite+# [ �� 5B   "   .+�5C   &   �� $ Horizontal =�61   $��Center=�6K   .+�5D   *   �� $��Название: ��64   $��16(�6F   $
��Aria)�6T   
�������6f   �� 5E   *   $��Medium"�64   $��14(�6H   $
��Aria)�6V   ���#C6C6C6 �� Padding$ 
0 2px 24 0q�6~   $��Center=�6�   �� 5F   .   �62   �� *	Info.Name+ �� 5G   .   $� — ��62    �� 5H   .   �62   �� *Info.WindowTitle+ �� Mode$ OneWay=�6b   5K   +   5L   '   �� 5M   &   $ 
Horizontal=�61   $��Center=�6J   .+�5N   *   �� $��Статус: ��64   
�������$��16(�5P   4   $
��Aria)�5Q   4   �� 5S   *   $��5 0 0 0q�5V   -   $��Center=�5W   -   ��5T   -     CalcBinding.Binding  *;9Info.IsRunning ? "Активно" : "Не активно"+��5U   -     *%#Info.IsRunning ? "#51FF44" : "Gray"+5W   -   �� 5Y   *   $ 5 0 0 0q�5Z   4   ���#51FF44$
��Aria)�5\   4   $��Center=�5]   4   ��5Y   4   �� *Info.Duration+ �� StringFormat$ {0:hh\:mm\:ss}��y�5^   4     *Info.IsRunning+   FalseToVisibility$ 	Collapsed=�5_   '   5`   #   5a      5b      5c      5d      5f      5g      5h      5i      