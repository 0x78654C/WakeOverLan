   M S B A M L   `   `   `  ���� >  :WOL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:WOLWOL      WOL.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     #localclr-namespace:WOL     -     � WindowStyle$
  None=� � AllowsTransparency . 
������  � Title$ Wake Over LAN��$��463.975��$��365.898�� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=� � Icon$ wol_ico.ico�� � 	Resources    MyWaterMarkStyle �� 
TargetType �� Property	 �� Value
 �� 
TargetType �� CornerRadius ��Name ��Name
 � Tag �� Triggers ]� 
Conditions �� Property �� Value �� 
TargetName P� Property P� Value  TextBoxBaseControlTemplate ��Name��%�  &	        &	 �    �� " ��.���  � ��	 �� "
 ��.R�� .+��� 
����������	#FF7D8683$��1q�$ 10���� $ PART_ContentHost��$��5,0,0,0q�$��Center=��� $ WaterMarkLabel��$��5,0,0,0q�#��z $��Center=�$��Center=�$y�	Collapsed=�
�������$��Arial)� ]�  ��  � ��$  ��./��� $ WaterMarkLabel�� � y�$	 Visible=�P�  � {� .  . ���  � ��
	 �iii��� "
 ��.R��� $ Bd��#��z��
���   �#��z��$ 10��._��� $ PART_ContentHost�� P�  � {� .  . ��� #	 � ��$ Bd�� � ���� #	 � �� � ��P�  � ��$
 Auto��. ���  � ��$		 100��P�  � ��$
 Auto��. ���  � ��$	 20��.���� $��1,1,1,1q�$ 8��
�������
���   �._��  J� RenderTransformOrigin$ 0.819,0.497&�$��0,0,0,-1q� � ColumnDefinitions �� �� $��0* �.+��� -   $��
TbIPTarget��#��[  $' !Enter your IP/Name address here..�� �� TextAlignment$ Center=�$��57,86,57,347q�$��16(� �� 	MaxLength$ 20��$��	Condensed&� �� TextWrapping$
 Wrap=��� -   $��TbMAC��#��[  $# Enter your MAC address here..��$ Center=�$��57,138,57,293q�$��16(�$��	Condensed&�$ 25��$
 Wrap=��� -   $��TbPort��#��[  $  Enter your WOL Port here..��$ Center=�$��57,191,57,241q�$��16(�$ 7��$��	Condensed&�$
 Wrap=��� -   $��BtnWake��  RoundCorner#���  $��Wake Target��$
��Left=�$��131,250,0,0q�$	��Top=�$	��101��$��43��$��14(�$
��Bold"����	#FFF7F7F7�� -   $��TbLog�� �� AcceptsReturn .  �� 
IsReadOnly . $ Center=�#��[ $��57,325,57,67q�$��14(�&   System.Windows.Media.TextOptions   TextFormattingMode$ Display=�$
 Wrap=��� $��Wake Over LAN��$
��Left=�$��
108,26,0,0q�$	��Top=�$��18(�$
��Bold"����	#FF094E7E�� -   $��BtnClose��#���  $��X��$
��Left=�$��
329,10,0,0q�$	��Top=�$��25��$��25��
�������
��������� -   $��lbStatus��$��Target Status:��$
��Left=�$��108,424,0,0q�$	��Top=��� -	   $��	lbDStatus��$�� Idle ��$
��Left=�$��194,424,0,0q�$	��Top=�$��72���� -
   $ lbDStatus_Copy��$
��v1.0��$
��Left=�$��332,437,-7,0q�$	��Top=�$��39��$��26��$��2��$��11(�