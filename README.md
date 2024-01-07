<div align="center"> 

  
![Notion](https://img.shields.io/badge/Notion-%23000000.svg?style=for-the-badge&logo=notion&logoColor=white) 
: [Notion][notionlink]

[notionlink]: https://www.notion.so/6ae307766bc84dd9b94ab463f08ebabe?pvs=4 "go notion"  


 </div>


#### <p align="center">아래는 해당 리포지토리의 간략한 설명입니다</p>

---

## 업적 시스템


<p align = "center"><img src="https://jidaeportfolio.s3.ap-northeast-2.amazonaws.com/Achievement.gif">
</p>


### UML


<p align = "center"><img src="https://jidaeportfolio.s3.ap-northeast-2.amazonaws.com/AchievementUML.png" height = "500px" weight = "100px">
</p>


### 개발과정
- 게임 플레이 시 스테이지 클리어 뿐 아니라 업적 시스템을 통해 **도전 목표를 제공**하기 위해 개발하게 되었습니다.
- **옵저버 패턴을 사용**하여 특정 클래스에서 조건을 만족하였을 경우, 업적 달성을 알리고 AcievementManager에서 해당 항목을 담고 있다 MainScene으로 넘어왔을 때 알람을 활성화 합니다.


### 간단한 클래스 설명
- [**AchievementManager.cs**](https://github.com/shji0318/StellarStudioCode/blob/main/Achievement/AchievementManager.cs)
  - 싱글톤 패턴을 사용
  - 업적 DB와 업적 달성 이벤트를 관리하는 클래스
 
  
- [**Achievement.cs**](https://github.com/shji0318/StellarStudioCode/blob/main/Achievement/Achievement.cs)
  - 업적 ID, Title, 달성 목표 데이터를 담고 있는 클래스
 
  
- [**AchievementSlot.cs**](https://github.com/shji0318/StellarStudioCode/blob/main/Achievement/Achievement_Slot.cs)
  - UI_Base를 상속 받아 Text 및 Image들을 해당 업적 달성 유무에 따라 유동적으로 관리
 
  


---

## 최적화


### 개발과정
- 로그 라이크 게임 특성상 수많은 몬스터가 생성되고 파괴되는데 **오브젝트 풀링**을 사용하여 이를 최적화 했습니다.
- 풀링뿐만 아니라 Unity2D 게임 개발에서 Batches를 줄이는 방법인 Sprite Atlas를 통한 image Packing
- 고정된 데이터 값에 대한 메모리를 효율적으로 사용하도록 ScriptableObject, 게임 빌드시 용량을 최소화하기 위해 AWS서버를 이용하여 Addressable을 사용했습니다.


### 간단한 클래스 설명
- [**PoolingManager.cs**](https://github.com/shji0318/StellarStudioCode/tree/main/Optimization)
  - 오브젝트 풀링을 관리하는 클래스
  - Dictionary 자료구조를 사용했으며 Key값으로 오브젝트의 이름(string) , Value로는 Pooling 클래스를 갖습니다.

  
- [**Pooling.cs**](https://github.com/shji0318/StellarStudioCode/blob/main/Optimization/Pooling.cs)
  - Pooling할 오브젝트를 Stack으로 관리
  - Pool의 단위로 사용
 

- [**Util.cs**](https://github.com/shji0318/StellarStudioCode/tree/main/Optimization)
  - 유니티에서 제공하는 함수를 랩핑을 통해 기능을 추가하여 사용하기 위한 전역 클래스입니다.
  - GameObject.Instantiate를 랩핑하여 Poolable이라는 컴포넌트를 갖은 오브젝트라면 풀링에서 관리하도록하여 사용했습니다.


---

## 플레이어 및 몬스터


<p align = "center"><img src="https://jidaeportfolio.s3.ap-northeast-2.amazonaws.com/Monster.gif">
</p>


### UML


<p align = "center"><img src="https://jidaeportfolio.s3.ap-northeast-2.amazonaws.com/CreatureUML.PNG.png" height = "500px" weight = "100px">
</p>


### 개발과정
- 게임의 가장 기본 요소인 플레이어와 몬스터는 아이템과 더불어 추후 개체가 추가될 가능성이 컸기 때문에 최대한 **코드 재사용성을 늘려 개발 효율을 늘리는 것을 목표**로 했습니다. 
- Monster와 Player가 공통으로 사용하는 특성 및 State를 추상 클래스인 CreatureController를 작성한 후, **상속하여 코드의 재사용성을 늘리도록 설계**했습니다.


### 간단한 클래스 설명
- [**CreatureController.cs**](https://github.com/shji0318/StellarStudioCode/blob/main/Achievement/CreatureController.cs)
  - Monster와 Player의 공통된 데이터를 갖는 추상 클래스
  - Monster와 Player 둘다 이동, 공격, 죽음과 같은 공통된 상태를 갖기 때문에 CreatureController에서 상태패턴을 통해 애니메이션 및 행동로직을 구현했습니다.
 
  
- [**MonsterController.cs**](https://github.com/shji0318/StellarStudioCode/blob/main/PlayerAndMonster/MonsterController.cs)
  - CreatureController를 상속 받는 클래스
  - 몬스터들 종류와 상관없는 공통된 기능들을 구현한 후, 몬스터 종류마다 개별적인 능력치는 **ScriptableObject를 통해 몬스터마다 스크립트를 생성하는 것이 아닌 하나의 스크립트로 재사용성을 늘렸습니다.**
    - 팩토리 패턴을 사용하여 상황에 따라 동적으로 필요로 하는 몬스터를 생성할 수 있도록 설계했습니다.
 
  
- [**MonsterSpawner.cs**](https://github.com/shji0318/StellarStudioCode/blob/main/PlayerAndMonster/MonsterSpawner.cs)
  - 몬스터를 생성하는 로직들을 갖고 있는 클래스
  - 일정 주기마다 몬스터를 스폰하는 로직과 특정 게임플레이 타임때마다 이벤트성으로 등장하는 몬스터들을 스폰하는 로직들로 구성되어 있습니다.
  - 해당 게임이 무한맵 개념이다 보니 고정 위치에 몬스터가 스폰될 경우 부자연스러움을 느낄 수 있다고 판단하여 플레이어 주변에 스폰되도록 하였습니다.
    - 카메라 범위 내에서 몬스터가 스폰될 경우 플레이어 입장에서 즉각적인 대응이 힘들기 때문에 적어도 카메라 범위 밖에서 스폰되어야 한다고 생각했습니다.
    - 이를 실행하기 위해 임의의 벡터를 구하는 공식인 **(노름 * Cos , 노름 * sin)을 이용해서 노름을 카메라의 범위로 두고 몬스터를 스폰**해서 해결했습니다.


- [**PlayerController.cs**](https://github.com/shji0318/StellarStudioCode/blob/main/PlayerAndMonster/PlayerController.cs)
  - CreatureController를 상속 받는 클래스
  - 플레이어의 인풋에 따른 캐릭터의 행동 및 애니메이션을 상태 패턴으로 구현했습니다.
 
    
---


## Save & Load


<p align = "center"><img src="https://jidaeportfolio.s3.ap-northeast-2.amazonaws.com/SaveLoad.gif">
</p>


### UML


<p align = "center"><img src="https://jidaeportfolio.s3.ap-northeast-2.amazonaws.com/Save%26LoadUML.PNG.png" height = "500px" weight = "100px">
</p>


### 개발과정
- 게임의 가장 기본 요소인 플레이어와 몬스터는 아이템과 더불어 추후 개체가 추가될 가능성이 컸기 때문에 최대한 **코드 재사용성을 늘려 개발 효율을 늘리는 것을 목표**로 했습니다. 
- Monster와 Player가 공통으로 사용하는 특성 및 State를 추상 클래스인 CreatureController를 작성한 후, **상속하여 코드의 재사용성을 늘리도록 설계**했습니다.


### 간단한 클래스 설명
- [**DataManager.cs**](https://github.com/shji0318/StellarStudioCode/blob/main/SaveAndLoad/DataManager.cs)
  - 플레이시 사용되는 데이터들을 관리하는 싱글톤 클래스
  - 업적, 옵션 세팅값, 골드 등 플레이어의 세이브 데이터 및 게임 플레이시 사용되는 Stage 데이터 등을 관리합니다.
 
  
- [**SaveData.cs**](https://github.com/shji0318/StellarStudioCode/blob/main/SaveAndLoad/SaveData.cs)
  - 업적, 옵션 세팅값, 골드, 상점 구매 목록 등을 사용자의 로컬에 저장
  - C#에서 제공하는 RijindaelManaed 클래스를 통하여 private key값을 이용하여 암호화하였습니다.
 
  
- [**SerializableDictionary.cs**](https://github.com/shji0318/StellarStudioCode/blob/main/SaveAndLoad/SerializableDictionary.cs)
  - SaveData를 구성할 때 업적 달성 목록을 저장해야했는데 직렬화된 데이터만 json으로 저장할 수 있기 때문에 **Dictionary를 저장하기 위해** 구현한 클래스
  - json으로 저장할 때 key값과 value값을 List로 받아온 후 직렬화해서 저장, 불러올 때는 역순인 데이터를 받아온 후 Dictionary형태로 데이터를 관리하게 됩니다.


- [**StageData.cs**](https://github.com/shji0318/StellarStudioCode/blob/main/PlayerAndMonster/StageData.cs)
  - 스테이지 플레이시 사용하는 데이터들을 관리하는 클래스
  - 해당 스테이지에서 획득한 아이템, 처치한 몬스터 수 등을 관리한 후 게임이 종료될 때 플레이어에게 제공
    
---


## Skill


<p align = "center"><img src="https://jidaeportfolio.s3.ap-northeast-2.amazonaws.com/Item.gif">
</p>


### UML


<p align = "center"><img src="https://jidaeportfolio.s3.ap-northeast-2.amazonaws.com/%EC%8A%A4%ED%82%AC%EB%8B%A4%EC%9D%B4%EC%96%B4%EA%B7%B8%EB%9E%A8.PNG.png" height = "500px" weight = "100px">
</p>


### 개발과정
- 뱀파이버 서바이벌 류 게임의 특성인 아이템을 획득 시 자동으로 스킬들을 사용하는 방식으로 구현하기로 했습니다. 
  - 이에 따라 스킬들의 실행 주기를 관리해야 했기에 우선순위 큐를 구현하여 이를 관리하였습니다.


### 간단한 클래스 설명
- [**PriorityQueue.cs**](https://github.com/shji0318/StellarStudioCode/blob/main/SkillAndItem/PriorityQueue.cs)
  - 아이템 획득 후, 자동으로 사용되는 스킬들을 관리하기 위해서 사용했습니다.
  - 이진 트리 구조를 갖으며 기준에 따라 자동으로 정렬하는 자료구조인 우선순위큐를 구현하여 사용했습니다.
    - 이때 기준을 스킬들의 다음 실행 주기를 기준으로 정렬하는 식으로 구현했습니다.  
 
  
- [**SkillManager.cs**](https://github.com/shji0318/StellarStudioCode/blob/main/SkillAndItem/SkillManager.cs)
  - 스킬의 등록, 사용, 능력치 강화 등을 관리하는 클래스    
 
  
- [**Skill.cs**](https://github.com/shji0318/StellarStudioCode/blob/main/SkillAndItem/Skill.cs)
  - 스킬의 기본적인 형태를 갖는 추상 클래스


---

