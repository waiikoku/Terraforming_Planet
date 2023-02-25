# Terraforming Planet
## UnityProject Version 2019.4.29f1
## Final Project ปวช.  [ปี 2019]
จัดทำโดยมีสมาชิกทีมทั้งหมด 3 คน
1.วิรัชภูมิ สุจารี
2.ภานุวงศ์ ตันทอง
3.อนุรักษ์ พักอุดม

Dillinger is a cloud-enabled, mobile-ready, offline-storage compatible,
Terraforming Planet เป็นเกมแนว City Building, Strategy ที่มีธีมเป็นแนวอวกาศ Settingอยู่บนพื้นผิวดาวเคราะห์คล้ายดาวอังคาร
- สร้างยูนิท
- เก็บเกี่ยวทรัพยากร
- สร้างสิ่งก่อสร้างที่อำนวยความสะดวก

## Features
- Select Unit like RTS
- Command Single/Multiple Unit
- Harvest Resources
- Build Extractor, Wind Turbine, Solar Cell, Motherbase
- Random Spawn Asteroid that generate resource node on impact

## Techniques

Terraforming Planet ใช้เทคนิคเสริมในการออกแบบเกม ดังนี้
- [ScriptableObject] Profile for Units, Resources and Buildings
- [Non-Convex Mesh Collider] Precise Collider for Run-time Stationary Building

## Controls

Move Camera - WASD ซ้ายขวาหน้าหลัง, Q/E หันขวาและซ้าย, RF ขึ้นและลง
Mouse - LMB เลือกวัตถุ, RMB สั่งยูนิทเดินไปยังตำแหน่งที่่คลิก|ยกเลิกคำสั่งสร้าง
ESC - Pause Menu เล่นต่อและกลับไปยังหน้าหลัก