#!/bin/bash

export PATH=$PATH:/d/Users/jbb/github/KSP-1-Mods

dirs="APPLE FIGS GRAPES LIME MANGO ORANGE PEAR PLUM PRUNES VSIndicator"


for d in $dirs; do

	localizer.sh  $d --prefix=LOC_${d} --outdir=GameData/FruitKocktail/${d}/Localization --numerictags $*

done
