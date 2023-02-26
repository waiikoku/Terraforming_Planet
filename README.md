# Terraforming Planet
![Banner](https://user-images.githubusercontent.com/48912221/221333702-7e172b9f-72f4-4a8c-927f-d8e53b110d31.png)

## UnityProject Version 2019.4.29f1
## Final Project ปวช.  [ปี 2019]
จัดทำโดยมีสมาชิกทีมทั้งหมด 3 คน
1.วิรัชภูมิ สุจารี
2.ภานุวงศ์ ตันทอง
3.อนุรักษ์ พักอุดม

ปล.โมเดลและไอคอนในเกมทำขึ้นมาเอง โดยอ้างอิงภาพในอินเทอร์เน็ต

Terraforming Planet เป็นเกมแนว City Building, Strategy ที่มีธีมเป็นแนวอวกาศ Settingอยู่บนพื้นผิวดาวเคราะห์คล้ายดาวอังคาร
- เก็บเกี่ยวทรัพยากร
- สร้างยูนิท
- สร้างสิ่งก่อสร้างที่อำนวยความสะดวก
- พัฒนา Colony ของคุณให้ยิ่งใหญ่

![Screenshot_Gameplay](https://user-images.githubusercontent.com/48912221/221333793-e4a89331-baaf-4508-a239-4a1e3adf9e22.png)

## Features
- Select Unit like RTS
- Command Single/Multiple Unit
- Harvest Resources
- Create More Units
- Build Extractor, Wind Turbine, Solar Cell, Motherbase
- Random Spawn Asteroid that generate resource node on impact

## Techniques

เทคนิคเสริมที่ใช้ในการออกแบบเกม
- [ScriptableObject] Profile for Units, Resources and Buildings
- [Non-Convex Mesh Collider] Precise Collider for Run-time Stationary Building

## Controls

| Control | รายละเอียด |
| ------ | ------ |
| Camera | WASD ซ้ายขวาหน้าหลัง, Q/E หันขวาและซ้าย, RF ขึ้นและลง |
| Mouse | LMB เลือกวัตถุ, RMB สั่งยูนิทเดินไปยังตำแหน่งที่่คลิก ยกเลิกคำสั่ง |
| ESC | Pause Menu เล่นต่อและกลับไปยังหน้าหลัก |

## Class Diagram [From ClassDesigner in VisualStudio]
![Code](https://user-images.githubusercontent.com/48912221/221333622-1d7cc511-5ca3-46cd-af70-618726ad7fe3.png)
ปล.Classบางอย่างก็ว่างเปล่า และAlgorithm การทำงานต่างๆที่เขียนขึ้นมานั้นเป็นความรู้เบื้องต้นที่เรียนรู้จากที่เรียนและศึกษาตัวตนเองหรือคัดลอกจากอินเทอร์เน็ต Performanceของเกมจึงทำงานไม่ได้ดีมากนัก

## Prefabs
![Prefabs](https://user-images.githubusercontent.com/48912221/221333634-67c05c39-398c-426a-b713-9f80e300bd14.png)

