
Not compatible with URP and HDRP.

How to use:
Add the script called "SelectionOutlineController.cs" to the camera you want to affect.
Then play the demo scene, click and hold on any object in the scene and it will show the outline.

Parameters:

Selection Mode:		1,Only outline the single object you select.	2 ,outline its children too.
Outline Type:	1,Outline the whole object .	2,Outline the whole object but colorize the occluded parts.	3,Only outline the visible parts .
Alpha Mode:		1,Read the alpha data of MainTex and cause holes.		2,Only outline the intact geometry.
Outline Color: The outline Color.
Occluded Color: The color that the occluded parts will be tinted with.
Outline Width:	Outline's width.
Outline Hardness: How much the outline color blend in.


An object must be given a collider and the collider must be put at the same place where the its renderer is.So that it can be selected (By Raycast).

Selection codes are in the Update function of the script. Write your own codes there if you want.

Warning: 
Remember to  assign two related shaders: PostprocessOutline and Target, into the ProjectSettings->Graphics->Always Included Shaders.
Otherwise this function would not work properly after you built your game.


Advice:
It'd be better that the main texture property of the selected object was named as "_MainTex", and the alpha data are stored in its Alpha channel.
So the transparent and clipped objects can be outlined properly.

By BookSun

================================================================
���������URP ,HDRP����
���ʹ�ã�
�ѱ�����ڵĽű� SelectionOutlineController.cs ��ӵ���ҪӰ�������ϡ�
Ȼ��������ʾ���������������ס��������һ���壬������ʾѡ����ߡ�
�������£�

Selection Mode(ѡ��ģʽ):		1,ֻѡ�е�������.	2 ,��ѱ�ѡ����������������嶼ѡ��.
Outline Type(���ģʽ):	1,Ϊ����������� .	2,Ϊ����������ߵ�����ڵ�����Ⱦɫ.	3,ֻΪ�ɼ�������� .
Alpha Mode(͸��ģʽ):		1,��ȡ����ͼ��Alpha��Ϣ������ϻ���˲����׶�.		2,ֻ��������ļ�����.
Outline Color(�����ɫ): ��ߵ���ɫ
Occluded Color(�ڵ���ɫ): ���ڵ����ֵ�Ⱦɫ
Outline Width(��߿��):	��ߵĿ��
Outline Hardness(���Ӳ��): ��ߵ���Ӳ�̶ȡ�

һ��������뱻�����ײ������ײ���Renderer��ͬһ��λ�ã��������ܱ�ѡ�в���ʾ��ߡ���ʹ��Raycastʵ�֣�
ѡ�е��߼����ƴ��붼�ڽű���Update�����ڣ������޸����Ա㡣

���棺
��ذ���ص�����shaders:PostprocessOutline and Target ����Ŀ�����У�����Ϊ��Զ������ ProjectSettings->Graphics->Always Included Shaders.
���������Ϸ����� �˹��ܻ�����쳣��

���飺
��ѡ�������shader�У�����ͼ��Property��ñ�����Ϊ"_MainTex"��������ͼ��Alpha��Ϣ����Aͨ������������͸������Ͳü�������ܱ���ȷ��ߡ�
