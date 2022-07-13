<template>
  <b-modal :id="id" :title="$t(bonus ? 'character.bonuses.editTitle' : 'character.bonuses.newTitle')" @hidden="setModel(bonus)">
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <bonus-type-select v-if="!bonus" :disabled="Boolean(bonus)" required v-model="type" />
        <template v-if="type">
          <attribute-select v-if="type === 'Attribute'" :disabled="Boolean(bonus)" required v-model="target" />
          <statistic-select v-else-if="type === 'Statistic'" :disabled="Boolean(bonus)" required v-model="target" />
          <skill-select v-else-if="type === 'Skill'" :disabled="Boolean(bonus)" required v-model="target" />
          <other-bonus-select v-else :disabled="Boolean(bonus)" required v-model="target" />
        </template>
        <template v-if="target">
          <form-field id="value" label="character.bonuses.value" required type="number" v-model.number="value" />
          <b-form-group>
            <b-form-checkbox v-model="permanent">{{ $t('character.bonuses.permanent') }}</b-form-checkbox>
          </b-form-group>
          <description-field :maxLength="1000" :rows="10" v-model="description" />
        </template>
      </b-form>
    </validation-observer>
    <template #modal-footer="{ cancel, ok }">
      <icon-button icon="ban" text="actions.cancel" @click="cancel()" />
      <icon-button v-if="bonus" :disabled="value === 0 || !hasChanges" icon="edit" text="actions.edit" variant="primary" @click="submit(ok)" />
      <icon-button v-else icon="plus" :disabled="value === 0 || !hasChanges" text="actions.add" variant="success" @click="submit(ok)" />
    </template>
  </b-modal>
</template>

<script>
import BonusTypeSelect from './BonusTypeSelect.vue'
import OtherBonusSelect from './OtherBonusSelect.vue'
import StatisticSelect from './StatisticSelect.vue'

export default {
  components: {
    BonusTypeSelect,
    OtherBonusSelect,
    StatisticSelect
  },
  props: {
    bonus: {
      type: Object,
      default: null
    },
    id: {
      type: String,
      required: true
    }
  },
  data: () => ({
    description: null,
    permanent: false,
    target: null,
    type: null,
    value: 0
  }),
  computed: {
    hasChanges() {
      return (
        this.value !== (this.bonus?.value ?? 0) ||
        this.permanent !== (this.bonus?.permanent ?? false) ||
        (this.description ?? '') !== (this.bonus?.description ?? '')
      )
    }
  },
  methods: {
    setModel(model) {
      this.type = model?.type ?? null
      this.target = model?.target ?? null
      this.value = model?.value ?? 0
      this.permanent = model?.permanent ?? false
      this.description = model?.description
    },
    async submit(callback = null) {
      try {
        if (await this.$refs.form.validate()) {
          this.$emit('ok', { callback, description: this.description, permanent: this.permanent, target: this.target, type: this.type, value: this.value })
          this.$refs.form.reset()
        }
      } catch (e) {
        this.handleError(e)
      }
    }
  },
  watch: {
    bonus: {
      deep: true,
      immediate: true,
      handler(bonus) {
        this.setModel(bonus)
      }
    },
    type() {
      this.target = null
    }
  }
}
</script>
