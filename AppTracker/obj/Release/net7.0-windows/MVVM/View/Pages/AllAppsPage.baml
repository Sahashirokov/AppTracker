   M S B A M L   `   `   `  ���� E  AAppTracker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:LauncherApp.MVVM.View.PagesLauncherApp.MVVM.View.Pages  F BCalcBinding, Version=2.5.2.0, Culture=neutral, PublicKeyToken=null>.clr-namespace:CalcBinding;assembly=CalcBindingCalcBinding -    'LauncherApp.MVVM.View.Pages.AllAppsPage   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     ;local)clr-namespace:LauncherApp.MVVM.View.Pages     <c.clr-namespace:CalcBinding;assembly=CalcBinding       N� Title$  AllAppsPage�� � DataContext �� *AllAppsViewModel+   ViewModelLocator �� Source# [  .��� ���#010B2F.+��  � RowDefinitions �� $��68 ��� $��* �.+�� $��0��$��68�����#46348E.+��� $$��Выбрать в ручную��  AddInPathButton#��[ $	��164��$��36��$��Center=�$
��left=�$��20 0 0 0q����� *ManualAddToFavoriteCommand+�� $��240px��$
��40px��$��Center=�$
��Left=�$��	200 5 5 5q�$��0 0 0 0q�  ModernTextBox#��[ ���� *	SearchApp+ �� UpdateSourceTrigger$ PropertyChanged=�.���� $��Автозапуск��$	��Top=�$��Right=�$��20 20 20 20q�$��16(�$��Arial)�$��Center=�
������� d� 	IsChecked �� *IsAutoStartEnabled+�� $��1��$��Stretch=� %� IsVirtualizing$
 True�� %� VirtualizationMode$ 	Recycling=�$
��True�����#040E27$��0q����� *Applications+���� .R���  �� Orientation$ 
Horizontal=�$��5q����#24262D.+��� $��42��$��42��$��10 0 0 0q����� *	Info.Icon+�� $��10,0,2,2q�$ Vertical=�$��Center=�.+��� $ 
Horizontal=�.+��� $,��&Добавить в избранное��  AddFavoriteApp#��[ ���� *" DataContext.AddToFavoriteCommand+	 �� RelativeSource	 �� 
 �� AncestorType
 ListView���� y��� *
IsFavorite+  BoolToVisibilityConverter �� 	Converter# [  �� ConverterParameter$ Reverse���� $��В избранном��
��� � �$��Center=�$
��Aria)�$��16(�$��5q�y��� *
IsFavorite+# [ �� .+��� $ Horizontal =�$��Center=�.+��� $��Название: ��$��16(�$
��Aria)�
��������� $��Medium"�$��14(�$
��Aria)����#C6C6C6 �� Padding$ 
0 2px 24 0q�$��Center=��� ��� *	Info.Name+ �� $� — �� �� ��� *Info.WindowTitle+ �� Mode$ OneWay=��� $ 
Horizontal=�$��Center=�.+��� $��Статус: ��
�������$��16(�$
��Aria)��� $��5 0 0 0q�$��Center=���  CalcBinding.Binding  *;9Info.IsRunning ? "Активно" : "Не активно"+��  *%#Info.IsRunning ? "#51FF44" : "Gray"+�� $ 5 0 0 0q����#51FF44$
��Aria)�$��Center=����� *Info.Duration+ �� StringFormat$ {0:hh\:mm\:ss}��y�  *Info.IsRunning+   FalseToVisibility$ 	Collapsed=�