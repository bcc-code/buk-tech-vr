#/bin/bash
mkdir -p temp
mkdir -p replaced
for tex in $(find textures/* -name emission -prune -o -type d -print )
do
  for oldMat in Materials/*.mat
  do
    mat=$(basename $oldMat | cut -d. -f1)
    texGuid=$(cat $tex/$mat.png.meta | grep -o "[[:xdigit:]]\{32\}")
    emissionGuid=$(cat textures/emission/$mat.png.meta | grep -o "[[:xdigit:]]\{32\}")
    newMat=temp/$(basename $tex)_$mat.mat
    cp $oldMat $newMat
    sed -i "s/m_Texture: {fileID: 2800000, guid: [0-9a-f]\{32\}, type: 3}/m_Texture: {fileID: 2800000, guid: $texGuid, type: 3}/" $newMat
    sed -i "s/m_Texture: {fileID: 2800000, guid: [0-9a-f]\{32\}, type: 3}/m_Texture: {fileID: 2800000, guid: $texGuid, type: 3}/" $newMat
  done
done
mv Materials/* replaced
mv temp/* Materials
rmdir temp
