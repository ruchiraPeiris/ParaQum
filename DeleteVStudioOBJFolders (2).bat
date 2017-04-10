xIF %1.==. GOTO END
%~d1
CD %1
FOR /R %%i IN (OBJ) DO RD /S/Q "%%i"
:END
