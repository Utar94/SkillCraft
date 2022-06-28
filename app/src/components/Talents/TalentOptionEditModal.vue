<template>
  <b-modal :id="id" :title="$t(option ? 'talent.options.editTitle' : 'talent.options.newTitle')" @hidden="setModel(option)">
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <name-field v-model="name" />
        <description-field :maxLength="1000" :rows="10" v-model="description" />
      </b-form>
    </validation-observer>
    <template #modal-footer="{ cancel, ok }">
      <icon-button icon="ban" text="actions.cancel" @click="cancel()" />
      <icon-button v-if="option" :disabled="!hasChanges" icon="edit" text="actions.edit" variant="primary" @click="submit(ok)" />
      <icon-button v-else icon="plus" :disabled="!hasChanges" text="actions.add" variant="success" @click="submit(ok)" />
    </template>
  </b-modal>
</template>

<script>
export default {
  props: {
    id: {
      type: String,
      required: true
    },
    option: {
      type: Object,
      default: null
    }
  },
  data: () => ({
    description: null,
    name: null
  }),
  computed: {
    hasChanges() {
      return (this.name ?? '') !== (this.option?.name ?? '') || (this.description ?? '') !== (this.option?.description ?? '')
    }
  },
  methods: {
    setModel(model) {
      this.description = model?.description
      this.name = model?.name
    },
    async submit(callback = null) {
      try {
        if (await this.$refs.form.validate()) {
          this.$emit('ok', { callback, description: this.description, name: this.name })
          this.$refs.form.reset()
        }
      } catch (e) {
        this.handleError(e)
      }
    }
  },
  watch: {
    option: {
      deep: true,
      immediate: true,
      handler(option) {
        this.setModel(option)
      }
    }
  }
}
</script>
