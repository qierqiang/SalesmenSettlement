# 结构

* EditFormBase    `DataBind`
  * AutoEditForm    `GenerateGroup`
    * AutoEditGroup    `GenerateControl`

# 执行顺序

* EdirFormBase
  * ctro > DataBind`(1)`
* AutoEditForm
  * ctro > GenerateGroup > AutoEditGroup.GenerateControl > DataBind`2`
* AutoEditGroup
  * ctor > GenerateControl > DataBind`2`
