<template>
  <div>
    <b-row>
      <name-field class="col" :id="id" label="race.names.label" placeholder="race.names.placeholder" :value="category" @input="$emit('renamed', $event)">
        <template #append>
          <icon-button v-if="status !== 'removed'" icon="trash-alt" text="actions.remove" variant="danger" @click="$emit('removed')" />
          <icon-button v-if="status === 'removed'" icon="undo" text="actions.restore" variant="warning" @click="$emit('restored')" />
        </template>
      </name-field>
      <form-field class="col" :id="`${id}_add`" label="race.names.add.label" placeholder="race.names.add.placeholder" v-model="name">
        <template #append>
          <icon-button :disabled="!name" icon="plus" variant="success" @click="addNames" />
        </template>
      </form-field>
    </b-row>
    <tag-list label="race.names.tags" :tags="tags" @remove="$emit('nameRemoved', $event)" />
  </div>
</template>

<script>
export default {
  props: {
    category: {
      type: String,
      default: ''
    },
    id: {
      type: String,
      required: true
    },
    status: {
      type: String,
      default: ''
    },
    values: {
      type: Array,
      default: () => []
    }
  },
  data: () => ({
    name: null
  }),
  computed: {
    tags() {
      return this.values.map((text, value) => ({ text, value }))
    }
  },
  methods: {
    addNames() {
      const names = this.name
        .split(/[,;]+/)
        .map(value => value.trim())
        .filter(value => value.length > 0)
      this.$emit('namesAdded', names)
      this.name = null
    }
  }
}
</script>
