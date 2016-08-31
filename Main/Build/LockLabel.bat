p4 label -o %1 > label.txt
bin\sed label.txt newlabel.txt unlocked locked
p4 label -i < newlabel.txt