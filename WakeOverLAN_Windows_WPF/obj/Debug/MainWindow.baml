   M S B A M L   `   `   `  ����>  :WOL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:WOLWOL  5   	       WOL.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   #localclr-namespace:WOL     5   	   V RGu.Wpf.Adorners, Version=2.1.0.0, Culture=neutral, PublicKeyToken=6dfb64f9972bd31d$adornershttp://gu.se/Adorners  5   	   -   5          � WindowStyle$
  None=�5	   	    � AllowsTransparency . 
������  � Title$ Wake Over LAN��5   	   $��463.975��6   $��365.898��60    � 
ResizeMode$ NoResize=�6@    � WindowStartupLocation$ CenterScreen=�6V    � Icon$ wol_ico.ico��6�    � 	Resources 5         TextBoxBaseControlTemplate �� 
TargetType ��Name	 �� CornerRadius
 ��Name �� Triggers P� Property P� Value �� Value �� 
TargetName �� Property��%�  &	        �� 5   
   " ��6=   .R�5      �� $ Bd��5      #��z��5      
���   �#��z��5      $	 10��6E   ._�5      �� $
 PART_ContentHost��6   5       5      P� 5       � {� .  . �5      �� # � ��63   $ Bd��6u    � ���� 5      # � ��63    � ��5      P� 5       � ��$
 Auto��6+   . �5      ��  � ��$	 100��61   5      P� 5       � ��$
 Auto��6,   . �5      ��  � ��$ 20��62   5      5!      5"      .��5$      �� $��1,1,1,1q�6   $	 8��6'   
�������
���   �._�5&   
   �  J� RenderTransformOrigin$ 0.819,0.497&�6   $��0,0,0,-1q�63    � ColumnDefinitions 5'      �� 5(      �� 5)      $��0* �6#   .+�5,      �� -   5        $��
TbIPTarget��5,   �     Gu.Wpf.Adorners.Watermark   Text$' !     Enter your target IP address��6      VisibleWhen$  EmptyAndNotKeyboardFocused=�6R    �� TextAlignment$ Center=�6�   #��[  6�   $��57,86,57,350q�6�   $��16(�6   �� 	MaxLength$ 20��6   �� TextWrapping$
 Wrap=�6!  �� 5-      -   5        $��TbMAC��5-   �   $&     Enter your target MAC address��6   $  EmptyAndNotKeyboardFocused=�6Q   $ Center=�6�   #��[  6�   $��57,138,57,298q�6�   $��16(�6   $��	Condensed&�6  $ 25��6&  $
 Wrap=�65  �� 5.      -   5        $��TbPort��5.   �   $$            Enter your WOL port��6   $  EmptyAndNotKeyboardFocused=�6O   $ Center=�6�   #��[  6�   $��57,191,57,245q�6�   $��16(�6�   $ 7��6  &   System.Windows.Media.TextOptions   TextFormattingMode$ Display=�6  $
 Wrap=�6D  �� 5/      -   5        $��BtnWake��5/   ;     RoundCorner#���  6   $��Wake Target��6J   $
��Left=�6`   $��131,250,0,0q�6{   $	��Top=�6�   $	��101��6�   $��43��6�   $��14(�6�   $
��Bold"�6�   ���	#FFF7F7F76�   �� 50      -   5        $��TbLog��50       �� 
IsReadOnly . $ Center=�65   #��[  6M   $��57,325,57,67q�6�   $��14(�6�   $ Display=�6�   $
 Wrap=�6�   �� 51      $��Wake Over LAN��6   $
��Left=�6,   $��
108,26,0,0q�6G   $	��Top=�6[   $��18(�6s   $
��Bold"�6�   ���	#FF094E7E6�   �� 52      -   5        $��BtnClose��52   ;   #���  6   $��X��6K   $
��Left=�6W   $��
329,10,0,0q�6r   $	��Top=�6�   $��25��6�   $��25��6�   
�������
�������6�   �� 53      -   5        $��lbStatus��53      $��Target Status:��6%   $
��Left=�6>   $��108,424,0,0q�6Y   $	��Top=�6n   �� 54      -	   5        $��	lbDStatus��54      $�� Idle ��6%   $
��Left=�66   $��194,424,0,0q�6Q   $	��Top=�6f   $��72��6~   �� 55      -
   5         ��Name$ lbDStatus_Copy��55      $
��v1.0��6,   $
��Left=�6;   $��332,437,-7,0q�6V   $	��Top=�6l   $��39��6�   $��26��6�   $��2��6�   $��11(�6�   57      59      5:      