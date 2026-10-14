# Vanilla/MO crop gross-yield baseline — 2026-10-14

## 2026-10-14 第二次数値監査 — 13変更作物＋MO対照2種・Grains七穀

**位置付け：** これは**優先再評価**であり、追加の`growDays`/`harvestYield`改定承認ではない。元の1.6収穫比率表と履歴は下節に保持する。実装対象は引き続きVanilla 6＋任意MO 7種の`growDays`のみ。Grainsの七穀・Vanilla稲・MO小麦、CCTOの耐寒性、自然植生には変更を加えない。

### 原本・仮定

- 対象Modの`Patches/CropGrowthBalance.xml`と`Compatibility/MedievalOverhaul/Patches/MOCropGrowthBalance.xml`（owner main `c74d0d4bf7dbb297392233aa40e3778c9c869902`）で**採用成長日数を直接照合**。
- ユーザー提供のMO原本`3219596926.zip`の**1.6だけ**を解析。`1.6/Defs/ThingDefs_Plants/Plants_Cultivated_Farm.xml` SHA-256 `f9a0b06fbb285063b3e6c79747483ed80e3590fcc185f91f350a58f3700d3f24`から当初の生育日数、`harvestYield`、`fertilitySensitivity`、`harvestAfterGrowth`を採録。`1.6/Defs/ThingDefs_Items/Items_Resource_RawPlant.xml` SHA-256 `119652fb2dd1936b9ecd715021119e54c1f86dcd631e0381b4e54d394f012717`で直接定義された栄養値だけを確認。MOの継承後の実効値・外部パッチは実ゲームの`ThingDef`で別途確認が必要。
- **Vanilla原本XMLは取得していない。** Vanilla 6種の元日数・標準収量・肥沃度感応度は[RimWorld Wikiの植物表](https://rimworldwiki.com/wiki/Crop)を外部の**二次資料**として参照。Grains七穀はGrains正本`Docs/Balance/Crops/SevenGrainFoodChainAudit.md`と`Docs/Design.md`の確定値。Wiki資料は1.6公式ゲームファイル・実ロード値の代用ではない。
- 比較指数は**土壌100%、最適温度、成長に有効な日照を1暦日当たり仮に13時間**、災害・腐敗・作業時間・スキル・搬送ゼロの単純化。`基準収穫数 / growDays`は**成長日基準**の生産量であり、暦日ではない。`初回満熟までの理想暦日 = growDays × 24 / 13`。実ゲームでは季節・緯度・温度により変動する。採り始め可能な部分成熟（低収量）を数えず、**100%満熟のみ**を計数。標準収穫数はPawnの収穫倍率を適用前の`harvestYield`。
- `harvestAfterGrowth=0.30`のMOトマト（対照：MOブドウ、果樹等にも同設定）は**収穫後30%生育段階を残して再生長するモデル**とし、次回以降の理論的な満熟間隔を初回の70%として扱う。これはXMLから得た想定で、**トマトの実際の再収穫Job・生存・出力は本専用Modの実ゲームでは未確認**。したがって単発収穫と長期連続収穫を混同しない。

### 元から存在する13種・MOの比較対照2種

| 提供元 | 作物 | 旧→現行`growDays` | 基準収穫数 | 収穫物/成長日 | 理想初回満熟日(13h) | 20暦日の理想満熟回数 |
|---|---|---:|---:|---:|---:|---:|
| Vanilla | ジャガイモ | 5.8→6 | 11 | 1.833 | 11.1 | 1 |
| Vanilla | トウモロコシ | 11.3→13 | 22 | 1.692 | 24.0 | **0** |
| Vanilla | イチゴ | 4.6→6 | 8 | 1.333 | 11.1 | 1 |
| Vanilla | 綿花（非食用） | 8→10 | 10 | 1.000 | 18.5 | 1 |
| Vanilla | ホップ（非食用） | 5→7 | 8 | 1.143 | 12.9 | 1 |
| Vanilla | スモークリーフ（非食用） | 7.5→9 | 9 | 1.000 | 16.6 | 1 |
| MO | タマネギ | 4→8 | 13 | 1.625 | 14.8 | 1 |
| MO | レンズ豆 | 3→8 | 10 | 1.250 | 14.8 | 1 |
| MO | ニンニク | 4→10 | 6 | 0.600 | 18.5 | 1 |
| MO | 亜麻（繊維原料） | 7→9 | 18 | 2.000 | 16.6 | 1 |
| MO | サトウキビ（加工原料） | 5.8→12 | 11 | 0.917 | 22.2 | **0** |
| MO | ニンジン | 3.25→6 | 6 | 1.000 | 11.1 | 1 |
| MO | トマト（再成長あり） | 4.25→7 | 8 | 1.143 | 12.9 | 1 |
| MO未変更 | キャベツ | 8→8 | 10 | 1.250 | 14.8 | 1 |
| MO未変更 | カボチャ | 8→8 | 20 | **2.500** | 14.8 | 1 |

- 理想日照13hモデルなら**10暦日で満熟する作物は対象13種・MO対照2種にない**。単純化した20日作期ではトウモロコシ・サトウキビが満熟できず収穫0だが、これは成長日数の実ゲーム保証ではない。参考としてGrains七穀の`growDays/基準収穫数`はアワ6/13、ヒエ6/12、キビ5/11、ソバ4/8、大麦10/22、小麦12/28、稲5/11。理想10暦日ではキビ・ソバ・稲が満熟可能という**加工・収量修飾なしの比較**になる。Grains七穀は収穫後に脱穀・殻取り等を必要とするため、食事生産効率をこれだけで順位付けしてはならない。
- 理想モデルとは別に**実ゲーム1.6制御環境**（[Project #38552728714](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38552728714)、owner`c63ceb21a7e6673e2e04284509704acd8f308de9`、`Plant.TickLong`を2,000tickごとに呼ぶ方式）では、10日目ジャガイモ**0.6768**・レンズ豆**約0.5076**、20日目ともに1.0000、Pawn収穫実績ジャガイモ14～15個・レンズ豆14個。高スキルPawnの収量修飾が掛かっており、**基準の11個・10個を14～15個へ変更する根拠ではない**。10日成長の割合は13hモデルと異なり（約9.75成長時間相当/日）、他13種へ同一の日照条件を機械的に転用してはならない。全季節・労働時間・植替え・低温を通した収量試験ではない。
- 肥沃度係数は`1 + fertilitySensitivity × (fertility - 1)`（例：100%土壌を1.0）。**50%係数が正でも播種可能とは限らない。** Vanilla主要栽培作物の最低肥沃度は一般に**70%**で、MO作物も元Defに`fertilityMin`がない場合はVanilla `PlantBase`を継承するため実ロード確認を要する。とくにEnvironmentの50% Thin Soilに「播種→収穫」を要求してしまうのは不適切。50%では播種可否を先にテストし、播種不可なら**正常な不可判定**として扱い、播種できるGrainsの対応作物と混同しない。実収量比較の基本土壌は70/100/140%、別途50%で適格性を測る。

### 食品と非食品を分けた暫定判定

- 可食作物の**生の標準収穫数/成長日**では、ジャガイモ1.833、トウモロコシ1.692、MOタマネギ1.625、レンズ豆1.250、キャベツ1.250、カボチャ**2.500**。MOカボチャはレンズ豆の**2倍**だが、これは同じ成長日数で得た**生の個数**に限定した事実。MOの両アイテムは原本1.6で`Nutrition=0.05`を明示しているため、**生の基準栄養/成長日**は各0.125と0.0625。一方、Grainsは未加工穀束をそのまま食べられるわけではなく、1:1一次加工・さらに料理Workがある。**カボチャやレンズ豆をそのまま穀物の可食・料理後価値と同一視しない**。
- MOニンニクは6個/10日で0.600個/成長日と低いが、薬味・副食用途であるため穀物と同じ生栄養効率を狙わない。レンズ豆は**播種スキル8**、MOキャベツは4、MOカボチャは8（原本1.6明示値）。レンズ豆が収穫量とスキル双方でキャベツに勝てない可能性があり、他のFood・レシピ・保存条件に役割があるかの検証が必要。
- MOトマトは生の8/7=1.143個/成長日だが、`harvestAfterGrowth=0.30`再成長が実ゲームでも有効なら、2回目以降の成熟間隔は理論上`7×(1−0.30)=4.9 growDays`、**8/4.9≒1.633個/成長日**。最初の栽培コストと植替え作業の削減も含め、収穫間隔・苗の生存を実ゲームで確認するまでは「トマトは低収量」と断定できない。
- MO亜麻は繊維系、サトウキビは加工・製糖原料、Vanilla綿花・ホップ・スモークリーフも非食料作物。**栄養量0として食料競争で敗者扱いするのは誤り**。加工Work、製品市場価値・保存性・用途を別の指標で見る。

### 次の数値判断ゲート（未採用の感度分析）

**数値修正の優先検証対象はレンズ豆・カボチャ・トマト**。現行の確定生育日数13件はいったん維持し、以下の例は**承認値ではなく単純な感度分析**として扱う。

| 仮定のみ | 現行→仮置き基準収穫数 | 生収穫数/成長日 | 変化 | 主な確認事項 |
|---|---:|---:|---:|---|
| MOレンズ豆を増産 | 10→12 | 1.250→1.500 | +20% | 高い播種スキル8に見合う役割、保存・レシピ、実収穫倍率 |
| MOカボチャを減産 | 20→16 | 2.500→2.000 | −20% | 高収量作物の個性とレンズ豆の差、畑・栽培時間の実測 |
| MOトマト | **変更なし** | 初回1.143／再成長理論1.633 | — | 初回と第2回の現物収穫、残存株・作業省略の真偽 |

この仮定値を本番XMLや設計書の**採用仕様には反映しない**。まず①Grainsなし・ありのロード後`harvestYield`/可食栄養値と収穫倍率、②50%播種不可・70/100/140%の現物収穫、③20暦日と長期連作の収量/植替えJob/加工Work、④レンズ豆vsカボチャvsキャベツvsトマトの実用役割を同一条件で検証する。比較表の勝敗を理由に他Mod・Grains・CCTO・VEの値を変更しない。追加`harvestYield`変更には所有者側の正式数値仕様承認とVE/非VE影響監査が必要。

### 再現手順

`python Tools/crop_balance_audit.py --mo-zip /path/to/3219596926.zip`（ユーザーの実際のMO原本ZIPがある場合）は、MO 1.6内部Defの数値一致と本Modの13個のXML上書きだけを照合し、JSONで13変更作物＋対照2種・Grains七穀のモデル値を出す。ZIPがない場合も本リポジトリの採用値と固定参考値を用いて実行できるが、**MO原本検証フラグはfalse**。`--growth-hours 9.75`は実制御試験の10日成長比率に近い**仮想感度分析**であり、全作物の季節実測ではない。`python -m unittest discover -s Tests -p 'test_*.py'`は計算の再現テストのみ。いずれもRimWorld実起動・ロード後Def・Unity ERRORを検証しない。

---

**Evidence:** real user-provided Medieval Overhaul archive `3219596926.zip`, **only** `3219596926/1.6/Defs/ThingDefs_Plants/Plants_Cultivated_Farm.xml` for crop values and `3219596926/1.6/Defs/ThingDefs_Items/Items_Resource_RawPlant.xml` for item Nutrition. Do not accidentally use the 1.4/1.5 variants of the same relative XML path. No executable game or current 1.7 MO source was inspected here. The production owner `Patches/CropGrowthBalance.xml` and `Compatibility/MedievalOverhaul/Patches/CropGrowthBalance.xml` remain unchanged.

## Gross source-based index

The following is **not calendar days, actual annual yield or tested native gameplay**. It is `harvestYield / effective growDays` at exactly fertility 100%, optimal growth temperature, common outdoor lighting opportunity, no sowing/harvest/movement/time loss and one planting/harvest cycle. `growDays` still needs game light, heat and fertility multipliers. "Nutrition index" is only shown when the **1.6 MO raw ThingDef explicitly defines `statBases/Nutrition=0.05`**; it is `harvestYield × 0.05 / growDays`, without digestion, meals, real Jobs or soil effects.

| Crop | Original growDays | New growDays | Original harvestYield | Units / growDay after patch | Explicit raw Nutrition | Gross Nutrition index |
|---|---:|---:|---:|---:|---:|---:|
| MO onions | 4 | 8 | 13 | 1.625 | 0.05 | 0.0813 |
| MO lentils | 3 | 8 | 10 | 1.250 | 0.05 | 0.0625 |
| MO garlic | 4 | 10 | 6 | 0.600 | 0.05 | 0.0300 |
| MO carrots | 3.25 | 6 | 6 | 1.000 | 0.05 | 0.0500 |
| MO tomatoes | 4.25 | 7 | 8 | 1.143 | 0.05 | 0.0571 |
| MO flax | 7 | 9 | 18 | 2.000 | not explicitly defined | — |
| MO sugarcane | 5.8 | 12 | 11 | 0.917 | not explicitly defined | — |
| MO cabbage — unchanged comparison | 8 | 8 | 10 | 1.250 | 0.05 | 0.0625 |
| MO pumpkin — unchanged comparison | 8 | 8 | 20 | 2.500 | 0.05 | 0.1250 |

**Comparison, not same economic cost:** Grains' approved seven-crop raw final grain-yield proxy at fertility 100% is Kibi 11/5=2.20, Awa 13/6≈2.17, Hie 12/6=2.00, buckwheat 8/4=2.00, barley 22/10=2.20, wheat 28/12≈2.33 and rice 11/5=2.20. For most *processed edible* Grains grains, `0.05 × output` is nominally 0.10–0.117 Nutrition/growDay; but grains require the explicit thresh/hull work and potentially milling and cooking. Existing grains' full source and 1:1 processing rule: [Grains SevenGrainFoodChainAudit](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/blob/main/Docs/Balance/Crops/SevenGrainFoodChainAudit.md). **Do not compare the raw values as interchangeable usable food** or conclude equal labor, storage or seasonal productivity.

## Findings and decisions not yet supported

- Lentils were originally fast at 3 growth-days and produce 10 items per harvest. The adopted 8 days reduces the gross 100%-fertility item-rate from 3.333 to 1.250 units/growth-day (−62.5%). This is a *static ratio*, not a measured harvest labor/annual calorie outcome.
- Pumpkin's unchanged 20 items / 8 days gives 2.5 units and 0.125 source Nutrition proxy / growDay, approximately twice lentils' 0.0625. That difference requires proper soil, sow/harvest work, storage, temperature and eaten/processed nutrition comparison before deciding any additional pumpkin patch.
- Garlic produces fewer nutrition units and may have different non-staple roles; do not tune every crop to the same edible output.
- Flax and sugarcane require their own processing or intended use and **must not** be represented as zero nutrition merely because the raw XML does not directly define 0.05; item inheritance/recipe use must be verified.
- Absolute crops per 10 or 20 calendar days **cannot be derived** from `growDays` alone: outdoor 13h plant growth opportunity, local temperature, terrain fertility, job time and harvest replanting determine elapsed and economic outcomes. Native map simulation remains mandatory.
- Vanilla raw harvestYield and Nutrition and 1.7 upstream versions have **not** been source-verified by this MO-only audit. No yield/meal/tolerance/research patch is authorized from this document.

## Follow-up acceptance in the owner repository

1. Verify in actual loaded game, for each of the four 1.6 profiles, all 13 effective growDays and original effective harvestYield/food Nutrition; fail explicitly on load-order conflicts. Native loaded-Def Pickle covers only the growth-day portion initially.
2. Add real 10/20-day sow/harvest cycles at at least 50%, 100%, 140% fertility, at known temperature/light with real Pawn jobs; record per-tile harvested nutrition and sow/harvest/processing work, not just source rates.
3. Compare MO lentils, cabbage/pumpkin, Vanilla potato/corn and the seven Grains crops under common profiles before proposing any further growth/yield changes. Separate nonfood resource crops.
4. Confirm any 1.7 values using a legitimate actual 1.7 MO/Vanilla provider and game before declaring supported versions. Keep the V1 target 1.6 only until this passes.
