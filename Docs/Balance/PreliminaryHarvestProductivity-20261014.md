# Vanilla/MO crop gross-yield baseline — 2026-10-14

## 2026-10-14 実ロード播種・収穫の基準作業量と収穫指数（15作物）

[Project isolated Linux 1.6 native run #38583040669](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38583040669)、CGB `f6e8190a60795b2195642ffcc76b2099d950e1cc`, Project `cdfbed1837f0edc47fc574a89ace203b5968aa39`, RimWorldLinux 1.6.4871 rev600, archived original-provider logs **11904645097**。変更13作物と、MOで未変更のキャベツ・カボチャ2作物について、実ロード済み`PlantProperties.sowWork`・`harvestWork`・`harvestYield`・収穫物`Nutrition`を取得。全4構成で各1/1 Pickle PASS、実ゲーム終了0。ただし元のMOプロバイダーを含む2構成ではFMODエラー各2件、**厳格判定2/4、ワークフローFAIL**。元の音声を変換・無効化した構成の結果ではない。

**新規判明：15作物すべて播種基準170、収穫基準200、初回1作付け基準作業370。** 既存の栄養・収穫数の違いは、初回の基準作業量を割り当てても縮まらない。以下の「初回原料栄養」は`harvestYield × 収穫物の実ロードNutrition`で、スキルによる増収・実際の食用適格性・加工・運搬・研究・土地条件・成熟可能日数は無視する**指標**である。

| 作物 | growDays | 基準収穫数 | 初回原料栄養指数 | 原料栄養／基準作業370 | 備考 |
|---|---:|---:|---:|---:|---|
| Vanilla ジャガイモ | 6 | 11 | 0.55 | 0.001486 | 生原料比較 |
| Vanilla トウモロコシ | 13 | 22 | 1.10 | 0.002973 | 長い生育期間 |
| Vanilla イチゴ | 6 | 8 | 0.40 | 0.001081 | 栽培技能5 |
| Vanilla 綿花 | 10 | 10 | 0.00 | 0.000000 | 非食品 |
| Vanilla ホップ | 7 | 8 | 0.40 | 0.001081 | 加工原料、直接可食としない |
| Vanilla スモークリーフ | 9 | 9 | 0.45 | 0.001216 | 加工原料、直接可食としない |
| MO タマネギ | 8 | 13 | 0.65 | 0.001757 | 栽培技能3 |
| MO レンズ豆 | 8 | 10 | 0.50 | 0.001351 | 栽培技能8 |
| MO ニンニク | 10 | 6 | 0.30 | 0.000811 | 栽培技能3 |
| MO 亜麻 | 9 | 18 | 0.90 | 0.002432 | 繊維原料。Nutrition stat≠可食 |
| MO サトウキビ | 12 | 11 | 0.55 | 0.001486 | 加工原料。Nutrition stat≠可食 |
| MO ニンジン | 6 | 6 | 0.30 | 0.000811 | 栽培技能4 |
| MO トマト | 7 | 8 | 0.40 | 0.001081 | **初回のみ**。実証済み再収穫では再播種をしない |
| MO キャベツ（未調整） | 8 | 10 | 0.50 | 0.001351 | 栽培技能4 |
| MO カボチャ（未調整） | 8 | 20 | 1.00 | 0.002703 | 栽培技能8・上級農業研究が必要 |

**評価：** レンズ豆とカボチャは両方`growDays=8`、同じ播種・収穫基準作業370、栽培技能8だが、基準原料栄養0.50対1.00で**2倍差が残る**。キャベツもレンズ豆と基準原料栄養・成長期間・作業量が同じで栽培技能のみ低い。カボチャは`DankPyon_AdvancedAgriculture`研究が必要で、この解禁費用・利用時期を考慮しない比較で収量調整を承認できない。トマトの同一株再収穫で必要な作業は「毎回370」ではなく、2回目以降は少なくとも播種分170が不要だが、専用支柱・再成熟期間・実Job作業時間を未計測のため反復収穫効率は未確定。

**制約：** `sowWork`／`harvestWork`はゲーム定義上の基準作業値であって、Pawnの実作業tick数ではない。食事調理・穀物脱穀、Grains同時実生産、季節変動、自然気温、実収量分布、研究開放、畑の枯死と病害は未検証。栄養の単純合計は直接摂取できない作物にも値が付くので、全作物を食料効率で順位付けしない。数値変更なし。

## 2026-10-14 実ロード栄養・収穫量の検証（原本1.6、Grainsなし）

[Project Native #38574555592](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38574555592)（artifact **11899008047**、owner `f470701c5b3a4bcb7366efb11132a3f2f03a2638`）にて、作物13種＋MO非変更比較対象2種の`harvestYield`、収穫物、継承を含む実ロード後`Nutrition`、最低肥沃度と播種スキルを4構成から取得。全4 Pickle検査成功・ゲーム終了0、ただしMOの独立したFMODエラー各2件により厳格な総合判定は**2/4 PASS・全体FAIL**。15種の正確な表は [Native16-CropGrowth](../Testing/Native16-CropGrowth-20261014.md) の「Loaded harvest resource evidence」節。

MOレンズ豆は**基準収穫量10、成長日8、生Nutrition0.05、栽培技能8**。未変更MOカボチャは**20、8、0.05、技能8**。従って`10×0.05/8=0.0625`と`20×0.05/8=0.125`は、**1回収穫を前提とする基準生栄養／成長日の静的指数**としては実ロード値の裏付けを得た。ただし上級農業研究、再植付け、作業時間、実際の季節、料理・処理後栄養は含まない。未変更MOキャベツの基準は10/8、原料Nutrition0.05、技能4。

**原本の直接定義とゲームでの継承値を区別**：MOトマト収穫物は1.6原本XMLに直接Nutritionを記載しないが、実ゲームで**0.05に継承解決**されるため単収穫比`8×0.05/7≈0.0571`は作れる。しかし同一株再収穫・支柱・自然季節を含む実栄養生産率とは言えない。MO亜麻・サトウキビも実ロード`Nutrition=0.05`だが直接可食・処理効率は未検証。全15種の実ロード`fertilityMin=0.70`、MOありで綿花の収穫物Defが変わることを確認した一方、50%土の現物播種テストは**未実施**。Grains併用、1.7、全季節の実収量と労働評価は引き続きOPEN、数値変更もなし。

## 2026-10-14 第二次数値監査 — 13変更作物＋MO対照2種・Grains七穀

**位置付け：** これは**優先再評価**であり、追加の`growDays`/`harvestYield`改定承認ではない。元の1.6収穫比率表と履歴は下節に保持する。実装対象は引き続きVanilla 6＋任意MO 7種の`growDays`のみ。Grainsの七穀・Vanilla稲・MO小麦、CCTOの耐寒性、自然植生には変更を加えない。

### 原本・仮定

- 対象Modの`Patches/CropGrowthBalance.xml`と`Compatibility/MedievalOverhaul/Patches/MOCropGrowthBalance.xml`（owner main `c74d0d4bf7dbb297392233aa40e3778c9c869902`）で**採用成長日数を直接照合**。
- ユーザー提供のMO原本`3219596926.zip`の**1.6だけ**を解析。`1.6/Defs/ThingDefs_Plants/Plants_Cultivated_Farm.xml` SHA-256 `f9a0b06fbb285063b3e6c79747483ed80e3590fcc185f91f350a58f3700d3f24`から当初の生育日数、`harvestYield`、`fertilitySensitivity`、`harvestAfterGrowth`を採録。`1.6/Defs/ThingDefs_Items/Items_Resource_RawPlant.xml` SHA-256 `119652fb2dd1936b9ecd715021119e54c1f86dcd631e0381b4e54d394f012717`で直接定義された栄養値だけを確認。MOの継承後の実効値・外部パッチは実ゲームの`ThingDef`で別途確認が必要。
- **Vanilla原本XMLは取得していない。** Vanilla 6種の元日数・標準収量・肥沃度感応度は[RimWorld Wikiの植物表](https://rimworldwiki.com/wiki/Crop)を外部の**二次資料**として参照。Grains七穀はGrains正本`Docs/Balance/Crops/SevenGrainFoodChainAudit.md`と`Docs/Design.md`の確定値。Wiki資料は1.6公式ゲームファイル・実ロード値の代用ではない。
- 比較指数は**土壌100%、最適温度、成長に有効な日照を1暦日当たり仮に13時間**、災害・腐敗・作業時間・スキル・搬送ゼロの単純化。`基準収穫数 / growDays`は**成長日基準**の生産量であり、暦日ではない。`初回満熟までの理想暦日 = growDays × 24 / 13`。実ゲームでは季節・緯度・温度により変動する。採り始め可能な部分成熟（低収量）を数えず、**100%満熟のみ**を計数。標準収穫数はPawnの収穫倍率を適用前の`harvestYield`。
- **解釈訂正とネイティブ観測（2026-10-14）：** MOトマトの`harvestAfterGrowth=0.30`は原本XML上の確実な属性値。ただしこの数値だけから「収穫時30%の成長を残す」「次回は70%時間で熟す」と**確定した扱いにはしない**。植物の描画・成熟段階にも関連する属性で、収穫後の株の存否とGrowth・次の収穫Jobは実ゲームで測る。旧モデルの再生長による加速は計算から除外した。以下の「理想満熟回数」は全作物とも**各収穫後に植え直す時間ゼロで100%まで育てる**仮想上限であり、トマトの特殊な再収穫ボーナスを含まない。

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
| MO | トマト（再収穫挙動未確認） | 4.25→7 | 8 | 1.143 | 12.9 | 1 |
| MO未変更 | キャベツ | 8→8 | 10 | 1.250 | 14.8 | 1 |
| MO未変更 | カボチャ | 8→8 | 20 | **2.500** | 14.8 | 1 |

- 理想日照13hモデルなら**10暦日で満熟する作物は対象13種・MO対照2種にない**。単純化した20日作期ではトウモロコシ・サトウキビが満熟できず収穫0だが、これは成長日数の実ゲーム保証ではない。参考としてGrains七穀の`growDays/基準収穫数`はアワ6/13、ヒエ6/12、キビ5/11、ソバ4/8、大麦10/22、小麦12/28、稲5/11。理想10暦日ではキビ・ソバ・稲が満熟可能という**加工・収量修飾なしの比較**になる。Grains七穀は収穫後に脱穀・殻取り等を必要とするため、食事生産効率をこれだけで順位付けしてはならない。
- 理想モデルとは別に**実ゲーム1.6制御環境**（[Project #38552728714](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38552728714)、owner`c63ceb21a7e6673e2e04284509704acd8f308de9`、`Plant.TickLong`を2,000tickごとに呼ぶ方式）では、10日目ジャガイモ**0.6768**・レンズ豆**約0.5076**、20日目ともに1.0000、Pawn収穫実績ジャガイモ14～15個・レンズ豆14個。高スキルPawnの収量修飾が掛かっており、**基準の11個・10個を14～15個へ変更する根拠ではない**。10日成長の割合は13hモデルと異なり（約9.75成長時間相当/日）、他13種へ同一の日照条件を機械的に転用してはならない。全季節・労働時間・植替え・低温を通した収量試験ではない。
- 肥沃度係数は`1 + fertilitySensitivity × (fertility - 1)`（例：100%土壌を1.0）。**50%係数が正でも播種可能とは限らない。** Vanilla主要栽培作物の最低肥沃度は一般に**70%**で、MO作物も元Defに`fertilityMin`がない場合はVanilla `PlantBase`を継承するため実ロード確認を要する。とくにEnvironmentの50% Thin Soilに「播種→収穫」を要求してしまうのは不適切。50%では播種可否を先にテストし、播種不可なら**正常な不可判定**として扱い、播種できるGrainsの対応作物と混同しない。実収量比較の基本土壌は70/100/140%、別途50%で適格性を測る。

### 食品と非食品を分けた暫定判定

- 可食作物の**生の標準収穫数/成長日**では、ジャガイモ1.833、トウモロコシ1.692、MOタマネギ1.625、レンズ豆1.250、キャベツ1.250、カボチャ**2.500**。MOカボチャはレンズ豆の**2倍**だが、これは同じ成長日数で得た**生の個数**に限定した事実。MOの両アイテムは原本1.6で`Nutrition=0.05`を明示しているため、**生の基準栄養/成長日**は各0.125と0.0625。一方、Grainsは未加工穀束をそのまま食べられるわけではなく、1:1一次加工・さらに料理Workがある。**カボチャやレンズ豆をそのまま穀物の可食・料理後価値と同一視しない**。
- MOニンニクは6個/10日で0.600個/成長日と低いが、薬味・副食用途であるため穀物と同じ生栄養効率を狙わない。レンズ豆は**播種スキル8**、MOキャベツは4、MOカボチャは8（原本1.6明示値）。**カボチャには上級農業`DankPyon_AdvancedAgriculture`研究が必要**。**トマトには中級農業`DankPyon_IntermediateAgriculture`と専用`DankPyon_Post`が必要**。作物間の加工・研究・設備差を無視した直接栄養比較では最終的な優劣を判断しない。
- MOトマトの基準8/7=1.143個/成長日は満熟時の単発比率。旧試算の**再成長理論1.633個/成長日は未検証であり撤回**する。トマトは`DankPyon_IntermediateAgriculture`研究と`DankPyon_VinePost` sowTag（専用設備`DankPyon_Post`）が必要なので、一般のGround栽培区画に直接播種する比較は不適切。実際の支柱で播種し、収穫後の株・成長率・第2回収穫Jobまで確認した後で年間価値を判断する。
- MO亜麻は繊維系、サトウキビは加工・製糖原料、Vanilla綿花・ホップ・スモークリーフも非食料作物。**栄養量0として食料競争で敗者扱いするのは誤り**。加工Work、製品市場価値・保存性・用途を別の指標で見る。


**2026-10-14追加観測・続報：** [Project #38561878157](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38561878157)（owner `1ee767c48e4072b8b5ce0592b4ab032da62943c5`）で、MO+CCTOの成熟トマトをテスト区画に直接生成し、Pawnが実際に収穫したところ、**10個・株は残存・成長率0.3000**を記録した。**最初の収穫直後に30%が残ることはこの条件で確認**した。ただし専用支柱での播種・次回の成長・2回目収穫は未実施のため、長期収量モデルで再成長率1.633を確定採用しない。初回はMO単独で生成Pawnの収穫倍率不足により試験前にFAILしたが、作業者選別を修正した再試験 [#38563498513](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38563498513) では**4構成ともPickle 1/1 PASS**（MO両構成のUnity FMOD2は残存）。MO単独・MO+CCTOとも、成熟トマトを実Pawnが収穫後、**各11個生成・同じ株が残り成長率0.3000**を確認した。続く実行 [#38565073363](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38565073363) で**同じトマト株の2回目の実Pawn収穫**をMOとMO+CCTOの両方で確認：初回11/10個、追加の制御暦10日後のGrowth0.9854、20日後Growth0.9997、**2回目10/11個**、再収穫後も元株が生存しGrowth0.3000。これは最初から成熟株を一時生成して2回とも実JobDriverで収穫した**植物限定の制御試験**であり、専用支柱播種、自然季節、年間収穫量、実運搬/料理労働とは別。単純な`8/4.9`の理論レートは現段階では本番数値変更に使用しない。Vanilla/CCTOジャガイモは0.7/1.0/1.4の土で播種Job提示を確認（収穫したのは1.0のみ）。詳細は [Native16-CropGrowth](../Testing/Native16-CropGrowth-20261014.md)。

**原本土壌での実収穫（2026-10-14、観測と制約）：** [Project #38568822665](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38568822665)（artifact 11897140747、owner `718c0bff6edda79390f6328d2796bf1c991c4c88`）で、ジャガイモ・レンズ豆を実際のPawnがGravel **70%**・Soil **100%**・SoilRich **140%**で播種し収穫した。全4構成Pickle 1/1 PASS、Unity ERRORは0/0/2/2でMOのFMODによるstrict FAILは継続。70/100/140%順の単株収穫数：ジャガイモVanilla **16/16/15**、Vanilla+CCTO **14/14/14**、レンズ豆MO **14/13/13**、MO+CCTO **13/14/14**。構成内は同じ実Pawnを使用。ただし**元の試験では土壌ごとに成長暦の開始日が異なり、季節の天空光が混入**したため、土壌ごとの*成長速度の優劣*はまだ採用しない。生育時の開始tickを共通化した正本[Native16-CropGrowth](../Testing/Native16-CropGrowth-20261014.md)の再実行結果が必要。50%肥沃度の実播種、MO支柱の実播種、Grains併用、自然季節の収量・加工労働は未検証。**今回も数値変更を承認しない**。

**条件を統一した実成長比較・収穫（2026-10-14）：** [Project #38570534129](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38570534129)（artifact 11898411270、owner `7a3c7a745deecd0ba0e58d0a207b9f10e2bf1ca6`）で、70%・100%・140%土壌それぞれの植物制御暦**開始tickと空光を一致**させた。実Pawnの播種・収穫Jobは4構成とも1/1 Pickle PASS。10暦日後の成長率（70/100/140順）：ジャガイモVanilla `0.5956/0.6768/0.7851`、Vanilla+CCTO `0.5955/0.6767/0.7850`、レンズ豆MO `0.4011/0.5076/0.6498`、MO+CCTO `0.4011/0.5077/0.6498`。**等時点では肥沃度の上昇と成長率の増加が整合**。レンズ豆の70%は20日目も約85.4%、140%は20日以内に成熟。70/100/140%各単株の実収穫数はジャガイモVanilla `15/15/15`、CCTO `13/14/14`、レンズ豆MO・MO+CCTOとも `14/13/14`。同構成では同じ実Pawnだが収量は平均ではない。Unity ERRORはVanilla/CCTO 0、MO/MO+CCTO 2（既知FMOD）のため**厳格受入は依然FAIL**。自然季節全体は進めておらず、季節を通じた年間生産性、研究支柱でのトマト実播種、Grains併用、作業/料理後Nutritionは未検証。追加数値変更は未採用。[詳細](../Testing/Native16-CropGrowth-20261014.md)。

### 次の数値判断ゲート（未採用の感度分析）

**数値修正の優先検証対象はレンズ豆・カボチャ・トマト**。現行の確定生育日数13件はいったん維持し、以下の例は**承認値ではなく単純な感度分析**として扱う。

| 仮定のみ | 現行→仮置き基準収穫数 | 生収穫数/成長日 | 変化 | 主な確認事項 |
|---|---:|---:|---:|---|
| MOレンズ豆を増産 | 10→12 | 1.250→1.500 | +20% | 高い播種スキル8に見合う役割、保存・レシピ、実収穫倍率 |
| MOカボチャを減産 | 20→16 | 2.500→2.000 | −20% | 高収量作物の個性とレンズ豆の差、畑・栽培時間の実測 |
| MOトマト | **変更なし** | 初回1.143／同一株2回収穫の実測確認、経済レート未算定 | — | 支柱と研究を満たす実播種、自然季節での連作と作業工数 |

この仮定値を本番XMLや設計書の**採用仕様には反映しない**。**追加ネイティブ試験の境界：** 既存の実播種・成熟・収穫100%土壌の試験に対し、原本Vanillaの70%砂利・100%土・140%肥沃土でのWorkGiver播種Job「提示可否」を追加する。50%は四構成に原本地形がない場合は`fertilityMin`ロード値だけを記録し、仮想地形を作らない。MOトマトについては原本の成熟株を隔離マップへ一時生成した**Pawnによる収穫Job**で株の生存とGrowthを観察するが、支柱上の実播種と自然季節の連作は未達ゲートと明記する。同一株2回の制御暦実収穫は既にPASS。カボチャの上級農業・トマトの中級農業/専用支柱のロード値を検査する。実行済みという記載ではない。まず①Grainsなし・ありのロード後`harvestYield`/可食栄養値と収穫倍率、②50%播種不可・70/100/140%の現物収穫、③20暦日と長期連作の収量/植替えJob/加工Work、④レンズ豆vsカボチャvsキャベツvsトマトの実用役割を同一条件で検証する。比較表の勝敗を理由に他Mod・Grains・CCTO・VEの値を変更しない。追加`harvestYield`変更には所有者側の正式数値仕様承認とVE/非VE影響監査が必要。

### 再現手順

`python Tools/crop_balance_audit.py --mo-zip /path/to/3219596926.zip`（ユーザーの実際のMO原本ZIPがある場合）は、MO 1.6内部Defの数値一致と本Modの13個のXML上書きだけを照合し、JSONで13変更作物＋対照2種・Grains七穀のモデル値を出す。ZIPがない場合も本リポジトリの採用値と固定参考値を用いて実行できるが、**MO原本検証フラグはfalse**。`--growth-hours 9.75`は実制御試験の10日成長比率に近い**仮想感度分析**であり、全作物の季節実測ではない。`python -m unittest discover -s Tests -p 'test_*.py'`は計算の再現テストのみ。いずれもRimWorld実起動・ロード後Def・Unity ERRORを検証しない。

---

**Evidence:** real user-provided Medieval Overhaul archive `3219596926.zip`, **only** `3219596926/1.6/Defs/ThingDefs_Plants/Plants_Cultivated_Farm.xml` for crop values and `3219596926/1.6/Defs/ThingDefs_Items/Items_Resource_RawPlant.xml` for item Nutrition. Do not accidentally use the 1.4/1.5 variants of the same relative XML path. No executable game or current 1.7 MO source was inspected here. The original MO tomato `DankPyon_RawTomatoes` in `Items_Resource_RawPlant.xml` has only MarketValue in its `statBases`, inheriting Nutrition from `PlantFoodRawBase`; source XML does not establish its effective Nutrition. The 0.05 / 0.0571 previously shown as **explicit** was incorrect. The native loaded-resource test must resolve the effective number. The production owner `Patches/CropGrowthBalance.xml` and `Compatibility/MedievalOverhaul/Patches/CropGrowthBalance.xml` remain unchanged.

## Gross source-based index

The following is **not calendar days, actual annual yield or tested native gameplay**. It is `harvestYield / effective growDays` at exactly fertility 100%, optimal growth temperature, common outdoor lighting opportunity, no sowing/harvest/movement/time loss and one planting/harvest cycle. `growDays` still needs game light, heat and fertility multipliers. "Nutrition index" is only shown when the **1.6 MO raw ThingDef explicitly defines `statBases/Nutrition=0.05`** (the original tomato item inherits its Nutrition and has no explicit value); it is `harvestYield × 0.05 / growDays`, without digestion, meals, real Jobs or soil effects.

| Crop | Original growDays | New growDays | Original harvestYield | Units / growDay after patch | Explicit raw Nutrition | Gross Nutrition index |
|---|---:|---:|---:|---:|---:|---:|
| MO onions | 4 | 8 | 13 | 1.625 | 0.05 | 0.0813 |
| MO lentils | 3 | 8 | 10 | 1.250 | 0.05 | 0.0625 |
| MO garlic | 4 | 10 | 6 | 0.600 | 0.05 | 0.0300 |
| MO carrots | 3.25 | 6 | 6 | 1.000 | 0.05 | 0.0500 |
| MO tomatoes | 4.25 | 7 | 8 | 1.143 | inherited; not explicit in MO 1.6 | — |
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
