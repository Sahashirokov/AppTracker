   M S B A M L   `   `   `  ���� E  AAppTracker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:LauncherApp.MVVM.View.PagesLauncherApp.MVVM.View.Pages  F BCalcBinding, Version=2.5.2.0, Culture=neutral, PublicKeyToken=null>.clr-namespace:CalcBinding;assembly=CalcBindingCalcBinding .    (LauncherApp.MVVM.View.Pages.FavoritePage   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     ;local)clr-namespace:LauncherApp.MVVM.View.Pages     <c.clr-namespace:CalcBinding;assembly=CalcBinding       N� Title$  Favorite�� � DataContext �� *	VmAppList+   ViewModelLocator �� Source# [  .��� .+���   ModernLoaderStyle#��[ 
�������y��� *	IsLoading+  BoolToVisibilityConverter �� 	Converter# [ �� $
��Auto=�
������ ���� *AppM+����  �� 
TargetType" ��.���  �� Property � �� �� Value$ Stretch=���  � ��$ 6q����� .R�� ���#24262D � ColumnDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $��* �.+��� $��0��$��42��$��42��$��10 0 0 0q����� *Icon+��  ��Name$ StartOrStop��$��1��  CommonStartStyle#��[ ���� *	IsRunning+  BoolToStartTextConverter# [ ���� *DataContext.ToggleAppCommand+	 �� RelativeSource	 �� 
 �� Mode$
 FindAncestor=� �� AncestorType ListBox���� �� $��3��$��Удалить��  CommonDeleteStyle#��[ ���� *DataContext.RemoveApp+	 �� $
 FindAncestor=� ListBox���� �� $��4�� �� Padding$ 2px 10px 8px 2pxq��� $�Название:��$��Arial)�$��16(�$
��Bold"�
������� ��  �� TextDecorations$  {�$��Segoe UI)�$��14(�
���������� *Name+�� $��5��$ 2px 10px 8px 2pxq��� $!�Первый запуск: ��$��Arial)�$��16(�$
��Bold"�
������� �� $��Segoe UI)�$��14(�
���������� *	StartTime+ �� Mode$ OneWay=� �� StringFormat$ {0:HH\:mm\:ss}���� $��6��$ 2px 10px 8px 2pxq��� $��Arial)�$��16(�$
��Bold"��  CalcBinding.Binding  *64IsRunning ?  'Активно': 'Не активно'+   Mode$ OneWay=���  * IsRunning ? "#51FF44" : "Gray"+�� $��7��$��Segoe UI)�$��14(�
�������$ 0px 10px 8px 0pxq���  *
Duration+$ OneWay=�   StringFormat$ {0:hh\:mm\:ss}��y�  *	IsRunning+   FalseToVisibility$ 	Collapsed=��� $��8��$ 0px 10px 8px 0pxq��� $!�Всего времени: ��$��Arial)�$��16(�$
��Bold"�
������� �� $��Segoe UI)�$��14(�
���������� *	TotalTime+$ {0:hh\:mm\:ss}������  �� 
TargetType" ��.R��� ._�� .+���  ��Name$ ScrollViewer��.���� �� $<��6У вас нет выбранных элементов��$��Arial)�$��18(�$
��Bold"�
������� �� TextAlignment$ Center=�$��Center=�y�`�    AllTrueToVisibilityConverter `� 	Converter# [ .1���  �� Path$ 	IsLoading�  InvertBoolConverter# [ �� $ Items.IsEmpty�	 ��  ListBox