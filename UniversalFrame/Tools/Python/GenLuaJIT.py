#coding-utf-8
import os

import sys

import shutil

currentPath=os.getcwd();

def ExcuseCommand(arg)
	if os.system(arg)!=0 :
		print('执行成功'+arg)
	else :
		print("执行失败"+arg)


if len (sys.argv)>1 :
	print(sys.argv[0])
	print(sys.argv[1])

def GetFileList(dir)
	dir=str(dir)
	if dir == "" :
		return []
	dir = dir .replace("/","\\");
	files=os.listdir(dir)
	results=[x for x in files if os.path.isfile(dir)]
	return results