   M S B A M L   `   `   `  ����>  :LPP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:LPPLPP  5   	       LPP.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   #localclr-namespace:LPP     5   	   -   5          � Title$	  LPP��5   	   $��709.543��6   $��1405.829��6&   .��5	      � $��0,0,-0.2,-42q�6   .+�5
   
   �� -   5         ��Name$ tbInput��5
      $
��Left=�6#   $��23��6>   $��	94,29,0,0q�6J    �� TextWrapping$
 Wrap=�6]   $�� ��6q   $	��Top=�6y   $
��1019��6�   �� 5   
   -   5         ��Name$ btnParse��5      $��Parse��6#   $
��Left=�63   $��1135,29,0,0q�6N   $	��Top=�6c   $��85��6{   $��23��6�   6�   �� 5   
   -   5        $ tbInfix��5      $
��Left=�6#   $��23��6>   $��	94,61,0,0q�6J   $
 Wrap=�6]   $�� ��6q   $	��Top=�6y   $
��1019��6�    �� 
IsReadOnly . {�.  6�   �� 5   
   -   5         ��Name$ imgTree��5      $
��Left=�6!   $	��388��6<   $��
40,265,0,0q�6I   $	��Top=�6]   $	��361��6u   �� 5   
   $��Input��6   $
��Left=�6    $��	50,29,0,0q�6;   $	��Top=�6N   �� 5   
   $��Infix��6   $
��Left=�6    $��	57,60,0,0q�6;   $	��Top=�6N   �� 5   
   -   5         ��Name$ lbTruthTable��5      $
��Left=�6(   $	��432��6C   $��406,221,0,0q�6P   $	��Top=�6e   $	��297��6}   �� 5   
   -   5        $ lbSimpleTable��5      $
��Left=�6)   $	��432��6D   $��722,221,0,0q�6Q   $	��Top=�6f   $	��292��6~   �� 5   
   -   5        $ tbNormalForm��5      $
��Left=�6(   $��23��6C   $��
96,127,0,0q�6O   $
 Wrap=�6c   $�� ��6w   $	��Top=�6   $
��1019��6�    . {�.  6�   �� 5   
   $��Normal form��6   $
��Left=�6&   $��
10,126,0,0q�6A   $	��Top=�6U    J� RenderTransformOrigin$ 0.709,2.434&�6m   �� 5   
   -	   5        $ 	tbNandify��5      $
��Left=�6%   $��23��6@   $��	96,95,0,0q�6L   $
 Wrap=�6_   $�� ��6s   $	��Top=�6{   $
��1019��6�    . {�.  6�   �� 5   
   $��Nandify��6   $
��Left=�6"   $��	37,93,0,0q�6=   $	��Top=�6P   �� 5   
   -
   5        $ tbNormalNandify��5      $
��Left=�6+   $��23��6F   $��
96,157,0,0q�6R   $
 Wrap=�6f   $�� ��6z   $	��Top=�6�   $
��1019��6�    . {�.  6�   �� 5   
   $��Nandify normal��6   $
��Left=�6)   $��	0,158,0,0q�6D   $	��Top=�6W   �� 5   
   -   5        $ lbHashes��5      $
��Left=�6$   $	��125��6?   $��1032,221,0,0q�6L   $	��Top=�6b   $	��295��6z   �� 5   
   -   5        $ 	lbUnbound��5      $
��Left=�6%   $	��119��6@   $��1135,87,0,0q�6M   $	��Top=�6b   $	��109��6z   �� 5   
   $��Unbound variables��6   $
��Left=�6,   $��1135,61,0,0q�6G   $	��Top=�6\   �� 5   
   -   5        $ tbSubstitute��5      $
��Left=�6(   $��23��6C   $��1249,87,0,0q�6O   $
 Wrap=�6d   $�� ��6x   $	��Top=�6�   $��78��6�   �� 5   
   -   5        $ btnSubstitute��5      $��
Substitute��6(   $
��Left=�6=   $��1249,115,0,0q�6X   $	��Top=�6n   $��78��6�   $��27��6�   6�   �� 5   
   -   5        $ btnTableaux��5      $��Semantic tableaux��6&   $
��Left=�6B   $��1249,29,0,0q�6]   $	��Top=�6r   $	��109��6�   $��23��6�   6�   �� 5   
   -   5         ��Name$ lblTautology��5      $��Is tautology? ��6&   $
��Left=�6?   $��
1249,0,0,0q�6Z   $	��Top=�6n   $ 
0.24,0.281&�6�   �� 5   
   -   5        $ btnShowTree��5      $��	Show tree��6&   $
��Left=�6:   $��1249,57,0,0q�6U   $	��Top=�6j   $	��109��6�   {�.  6�   5       5!      