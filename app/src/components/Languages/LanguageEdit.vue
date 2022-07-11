<template>
  <b-container>
    <h1 v-text="title" />
    <status-detail
      v-if="language"
      :createdAt="new Date(language.createdAt)"
      :deletedAt="language.deletedAt ? new Date(language.deletedAt) : null"
      :updatedAt="language.updatedAt ? new Date(language.updatedAt) : null"
    />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <b-row>
          <name-field class="col" v-model="name" />
          <form-field
            class="col"
            id="script"
            label="language.script.label"
            :maxLength="100"
            :minLength="0"
            placeholder="language.script.placeholder"
            v-model="script"
          />
        </b-row>
        <b-form-group>
          <b-form-checkbox v-model="exotic">{{ $t('language.exotic') }}</b-form-checkbox>
        </b-form-group>
        <form-field
          id="typicalSpeakers"
          label="language.typicalSpeakers.label"
          :maxLength="100"
          :minLength="0"
          placeholder="language.typicalSpeakers.placeholder"
          v-model="typicalSpeakers"
        />
        <description-field v-model="description" />
        <div class="my-2">
          <icon-submit v-if="language" class="mx-1" :disabled="!hasChanges || loading" icon="save" :loading="loading" text="actions.save" variant="primary" />
          <icon-submit v-else class="mx-1" :disabled="!hasChanges || loading" icon="plus" :loading="loading" text="actions.create" variant="success" />
          <icon-button class="mx-1" icon="arrow-left" text="actions.back" :to="{ name: 'LanguageList' }" />
        </div>
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
import { createLanguage, getLanguage, updateLanguage } from '@/api/languages'

export default {
  data: () => ({
    description: null,
    exotic: false,
    language: null,
    loading: false,
    name: null,
    script: null,
    typicalSpeakers: null
  }),
  computed: {
    hasChanges() {
      return (
        (this.name ?? '') !== (this.language?.name ?? '') ||
        this.exotic !== (this.language?.exotic ?? false) ||
        (this.script ?? '') !== (this.language?.script ?? '') ||
        (this.typicalSpeakers ?? '') !== (this.language?.typicalSpeakers ?? '') ||
        (this.description ?? '') !== (this.language?.description ?? '')
      )
    },
    payload() {
      return {
        description: this.description,
        exotic: this.exotic,
        name: this.name,
        script: this.script,
        typicalSpeakers: this.typicalSpeakers
      }
    },
    title() {
      return this.language?.name ?? this.$i18n.t('language.title')
    }
  },
  methods: {
    setModel(model) {
      this.language = model
      this.description = model.description
      this.exotic = model.exotic
      this.name = model.name
      this.script = model.script
      this.typicalSpeakers = model.typicalSpeakers
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            if (this.language) {
              const { data } = await updateLanguage(this.language.id, this.payload)
              this.setModel(data)
              this.toast('success', 'language.updated')
            } else {
              const { data } = await createLanguage(this.payload)
              this.setModel(data)
              this.toast('success', 'language.created')
            }
            this.$refs.form.reset()
          }
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
      }
    }
  },
  async created() {
    const id = this.$route.params.id
    if (id && id !== 'new') {
      try {
        const { data } = await getLanguage(id)
        this.setModel(data)
      } catch (e) {
        if (e.status === 404) {
          return this.$router.push({ name: 'NotFound' })
        }
        this.handleError(e)
      }
    }
  }
}
</script>
